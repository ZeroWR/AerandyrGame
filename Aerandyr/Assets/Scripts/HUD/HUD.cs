using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HUD : MonoBehaviour
{
	public Text HealthText = null;
	public Canvas DialogPanel = null;
	public Text DialogSpeakerName = null;
	private Color defaultSpeakerNameColor;
	public Text DialogText = null;
	private IsoCharacterController controller = null;
	public IsoCharacterController Controller
	{
		get { return controller; }
		set
		{
			controller = value;
			if(this.questObjectivesController)
			{
				this.questObjectivesController.Controller = value;
			}
		}
	}
	private float nextKeyDownTime = 0.0f;
	public float KeyPressInterval = 0.5f;
	private DialogPlayer dialogPlayer;
	public Canvas QuestPanel = null;
	public AnimationCurve QuestAnimation = null;
	private AnimationCurve closingAnimation = new AnimationCurve();
	private Vector2 originalQuestPanelPosition;
	private RectTransform questPanelRectTransform;
	private UnityEvent questAnimationDoneEvent = new UnityEvent();
	private EasyTween questTween;
	public Canvas CurrentQuestPanel = null;
	private QuestObjectivesController questObjectivesController;

	// Start is called before the first frame update
	void Start()
    {
		this.DialogPanel.enabled = false;
		this.DialogText.text = string.Empty;
		if(this.dialogPlayer == null)
			this.dialogPlayer = this.gameObject.AddComponent<DialogPlayer>();

		this.dialogPlayer.SectionStarted += this.SectionStarted;
		this.dialogPlayer.TextUpdated += this.TextUpdated;
		if (this.DialogSpeakerName != null)
			this.defaultSpeakerNameColor = this.DialogSpeakerName.color;

		if (this.QuestPanel)
		{
			this.QuestPanel.enabled = false;
			this.questPanelRectTransform = this.QuestPanel.GetComponent<RectTransform>();
			this.originalQuestPanelPosition = this.questPanelRectTransform.anchoredPosition;
			this.questTween = this.QuestPanel.GetComponent<EasyTween>();
			this.questTween.enabled = false;
		}

		if(this.CurrentQuestPanel)
		{
			this.CurrentQuestPanel.enabled = false;
			var objectivesPanel = this.CurrentQuestPanel.GetComponentsInChildren<Canvas>().SingleOrDefault(x => x.name == "ObjectivesPanel");
			if (objectivesPanel != null)
			{
				this.questObjectivesController = objectivesPanel.GetComponent<QuestObjectivesController>();
				if(this.questObjectivesController != null)
				{
					this.questObjectivesController.Controller = this.Controller;
				}
			}
		}
		
		this.questAnimationDoneEvent.AddListener(new UnityAction(this.OnQuestAnimationDone));
	}
    // Update is called once per frame
    void Update()
    {
		UpdateHealthText();
    }
	public void ProcessInput()
	{
		if (this.IsInDialog && this.CanProcessKeyPress && Input.GetKeyDown(KeyCode.E))
		{
			if(this.dialogPlayer.IsFinishedPlaying)
				CloseDialog();
			else
				this.dialogPlayer.ProcessInput();
		}
	}
	private void UpdateHealthText()
	{
		if (this.HealthText != null && this.Controller != null && this.Controller.PlayerCharacter != null)
		{
			this.HealthText.text = this.Controller.PlayerCharacter.Health.ToString();
		}
	}
	#region Dialog
	public DialogPlayer ShowDialog(Dialog dialog)
	{
		this.DialogText.text = string.Empty;
		this.DialogPanel.enabled = true;
		this.nextKeyDownTime = Time.time + this.KeyPressInterval;
		if (this.dialogPlayer.LoadDialog(dialog))
			this.dialogPlayer.PlayDialog();
		return this.dialogPlayer;
	}
	public void CloseDialog()
	{
		if(!this.dialogPlayer.IsFinishedPlaying)
			this.dialogPlayer.StopPlaying();
		this.DialogText.text = string.Empty;
		this.DialogSpeakerName.text = string.Empty;
		this.DialogSpeakerName.color = this.defaultSpeakerNameColor;
		this.DialogPanel.enabled = false;
		this.OnDialogFinished(this.dialogPlayer.CurrentDialog);
	}
	public bool IsInDialog { get { return this.DialogPanel != null && this.DialogPanel.enabled; } }
	private bool CanProcessKeyPress { get { return Time.time >= this.nextKeyDownTime; } }

	public delegate void DialogFinishedEventHandler(object sender, Dialog dialog);
	public DialogFinishedEventHandler DialogFinished;
	protected void OnDialogFinished(Dialog dialog)
	{
		if (this.DialogFinished != null)
			this.DialogFinished.Invoke(this, dialog);
	}
	#endregion

	#region Quests
	public void ReceivedQuest(Quest quest)
	{
		if (!this.QuestPanel)
			return;
		this.QuestPanel.enabled = true;
		if (!questTween)
			return;
		var title = this.QuestPanel.GetComponentsInChildren<Text>().First(x => x.name == "QuestTitle");
		if (title != null)
			title.text = quest.Name;
		var rectTransform = this.questPanelRectTransform;
		if (!rectTransform)
			return;
		var startPosition = rectTransform.anchoredPosition;
		//startPosition.x += (Screen.width * 0.5f);
		startPosition.x += Screen.width;
		var endPosition = rectTransform.anchoredPosition;
		endPosition.x -= Screen.width;
		this.questPanelRectTransform.anchoredPosition = startPosition; //Give it some help, so we don't flicker.
		questTween.SetAnimationProperties
		(
			new UITween.AnimationParts
			(
				UITween.AnimationParts.State.CLOSE,
				false,
				false,
				true,
				UITween.AnimationParts.EndTweenClose.NOTHING,
				UITween.AnimationParts.CallbackCall.END_OF_INTRO_ANIM,
				this.questAnimationDoneEvent,
				this.questAnimationDoneEvent
			)
		);
		questTween.SetAnimatioDuration(3f);
		questTween.SetAnimationPosition(startPosition, endPosition, this.QuestAnimation, closingAnimation);
		questTween.enabled = true;
		questTween.OpenCloseObjectAnimation();
	}
	public void OnQuestAnimationDone()
	{
		this.questPanelRectTransform.anchoredPosition = this.originalQuestPanelPosition;
		this.QuestPanel.enabled = false;
		questTween.enabled = false;

		var currentQuest = this.Controller.CurrentQuest;
		if (currentQuest == null)
			return;
		var currentQuestTitle = this.CurrentQuestPanel.GetComponentInChildren<Text>();
		if (currentQuestTitle != null)
			currentQuestTitle.text = currentQuest.Name;
		this.CurrentQuestPanel.enabled = true;
	}
	#endregion

	#region Events
	private void SectionStarted(object sender, DialogSection section)
	{
		if (this.DialogSpeakerName == null)
			return;

		this.DialogSpeakerName.text = string.IsNullOrEmpty(section.SpeakerName) ? string.Empty : section.SpeakerName;
		var speakerNameColor = this.defaultSpeakerNameColor;
		if(!string.IsNullOrEmpty(section.SpeakerNameColor))
		{
			if(!ColorUtility.TryParseHtmlString(section.SpeakerNameColor, out speakerNameColor))
				speakerNameColor = this.defaultSpeakerNameColor;
		}
		this.DialogSpeakerName.color = speakerNameColor;
	}
	private void TextUpdated(object sender, string updatedText)
	{
		if (DialogText != null)
			DialogText.text = updatedText;
	}
	#endregion
}

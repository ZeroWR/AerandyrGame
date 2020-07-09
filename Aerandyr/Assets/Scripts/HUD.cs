using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public Text HealthText = null;
	public Canvas DialogPanel = null;
	public Text DialogSpeakerName = null;
	private Color defaultSpeakerNameColor;
	public Text DialogText = null;
	public Player Player { get; set; }
	private float nextKeyDownTime = 0.0f;
	public float KeyPressInterval = 0.5f;
	private DialogPlayer dialogPlayer;
	
	// Start is called before the first frame update
	void Start()
    {
		this.DialogPanel.enabled = false;
		this.DialogText.text = string.Empty;
		if(this.dialogPlayer == null)
			this.dialogPlayer = this.gameObject.AddComponent<DialogPlayer>();

		this.dialogPlayer.SectionStarted += this.SectionStarted;
		this.dialogPlayer.SectionFinished += this.SectionFinished;
		this.dialogPlayer.StartedPlaying += this.PlayerStarted;
		this.dialogPlayer.FinishedPlaying += this.PlayerFinished;
		this.dialogPlayer.TextUpdated += this.TextUpdated;
		if (this.DialogSpeakerName != null)
			this.defaultSpeakerNameColor = this.DialogSpeakerName.color;
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
		if (this.HealthText != null && this.Player != null)
		{
			this.HealthText.text = this.Player.Health.ToString();
		}
	}
	public void ShowDialog(Dialog dialog)
	{
		this.DialogText.text = string.Empty;
		this.DialogPanel.enabled = true;
		this.nextKeyDownTime = Time.time + this.KeyPressInterval;
		if (this.dialogPlayer.LoadDialog(dialog))
			this.dialogPlayer.PlayDialog();
	}
	public void CloseDialog()
	{
		if(!this.dialogPlayer.IsFinishedPlaying)
			this.dialogPlayer.StopPlaying();
		this.DialogText.text = string.Empty;
		this.DialogSpeakerName.text = string.Empty;
		this.DialogSpeakerName.color = this.defaultSpeakerNameColor;
		this.DialogPanel.enabled = false;
	}
	public bool IsInDialog { get { return this.DialogPanel != null && this.DialogPanel.enabled; } }
	private bool CanProcessKeyPress { get { return Time.time >= this.nextKeyDownTime; } }

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

	private void SectionFinished(object sender, DialogSection section)
	{

	}

	private void PlayerStarted(object sender)
	{

	}

	private void PlayerFinished(object sender)
	{
	}

	private void TextUpdated(object sender, string updatedText)
	{
		if (DialogText != null)
			DialogText.text = updatedText;
	}
	#endregion
}

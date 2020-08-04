using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gershigglefertz : MonoBehaviour, IInteractable
{
	protected Animator animator;
	protected bool isJiggling = false;
	protected float nextJiggleTime = 0.0f;
	protected Dialog ourDialog;
	protected Quest ourQuest;
	protected string dialogName = "Gershigglefertz";
	protected string questName = "Test Quest";
	// Start is called before the first frame update
	protected virtual void Start()
    {
		animator = GetComponent<Animator>();
		SetNextJiggleTime();
		InitDialog();
		InitQuest();
	}

    // Update is called once per frame
    void Update()
    {
		if (nextJiggleTime <= Time.time && !isJiggling)
			Jiggle();
    }

	private void Jiggle()
	{
		if (isJiggling)
			return;
		animator.StopPlayback();
		animator.Play("Jiggle");
		isJiggling = true;
	}

	private void OnJiggleDone()
	{
		isJiggling = false;
		animator.Play("Idle");
		SetNextJiggleTime();
	}

	private void SetNextJiggleTime()
	{
		nextJiggleTime = Time.time + Random.Range(1.0f, 3.0f);
	}
	public virtual void Interact(Object sender)
	{
		if (!(sender is IsoCharacterController))
			return;
		var senderPlayer = sender as IsoCharacterController;
		//This sucks.  Have to do this because Start() isn't called in any order.
		if (this.ourDialog == null)
			InitDialog();
		if (this.ourQuest == null)
			InitQuest();
		if (!senderPlayer.HUD || this.ourDialog == null || this.ourQuest == null)
			return;

		var isInPart2 = ourQuest.CurrentSection == ourQuest.Sections[2];
		if (senderPlayer.CurrentQuest == ourQuest && !isInPart2)
		{
			senderPlayer.ShowDialog(new TransientDialog("Please talk to my brother."));
			return;
		}
		
		var dialogToPlay = (isInPart2 && !ourQuest.CurrentSection.Objectives[0].IsDone) ? Interactions.Instance.GetDialog(this.dialogName + "Part2") : this.ourDialog;

		if(isInPart2)
			ourQuest.CurrentSection.Objectives[0].IsDone = true;

		HUD.DialogFinishedEventHandler dialogFinishedHandler = null;
		dialogFinishedHandler = (s, dialog) =>
		{
			if (dialog != dialogToPlay)
				return;
			if (senderPlayer.CurrentQuest != ourQuest)
			{
				ourQuest.CurrentSection = ourQuest.Sections.First();
				senderPlayer.ReceivedQuest(ourQuest);
			}
			else if(isInPart2)
			{
				ourQuest.CurrentSection = ourQuest.Sections[3];
			}
			senderPlayer.HUD.DialogFinished -= dialogFinishedHandler;
		};
		senderPlayer.HUD.DialogFinished += dialogFinishedHandler;
		senderPlayer.ShowDialog(dialogToPlay);
	}
	public bool CanInteract(Object sender)
	{
		return sender is IsoCharacterController;
	}
	protected void InitQuest()
	{
		ourQuest = QuestManager.Instance.Quests.Find(x => x.Name == this.questName);
	}
	protected void InitDialog()
	{
		ourDialog = Interactions.Instance.GetDialog(this.dialogName);
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gershigglefertz : MonoBehaviour, IInteractable
{
	private Animator animator;
	private bool isJiggling = false;
	private float nextJiggleTime = 0.0f;
	private Dialog ourDialog;
	private Quest ourQuest;
    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
		SetNextJiggleTime();
		ourDialog = Interactions.Instance.GetDialog("Gershigglefertz");
		ourQuest = QuestManager.Instance.Quests.Find(x => x.Name == "Test Quest");
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
	public void Interact(Object sender)
	{
		if(sender is IsoCharacterController)
		{
			var senderPlayer = sender as IsoCharacterController;
			//This sucks.  Have to do this because Start() isn't called in any order.
			if(this.ourDialog == null)
				ourDialog = Interactions.Instance.GetDialog("Gershigglefertz");
			if(this.ourQuest == null)
				ourQuest = QuestManager.Instance.Quests.Find(x => x.Name == "Test Quest");
			if (!senderPlayer.HUD || this.ourDialog == null || this.ourQuest == null)
				return;

			var dialogPlayer = senderPlayer.HUD.ShowDialog(this.ourDialog);
			HUD.DialogFinishedEventHandler dialogFinishedHandler = null;
			dialogFinishedHandler = (s, dialog) =>
			{
				if (dialog != ourDialog)
					return;
				ourQuest.CurrentSection = ourQuest.Sections.First();
				senderPlayer.ReceivedQuest(ourQuest);
				senderPlayer.HUD.DialogFinished -= dialogFinishedHandler;
			};
			senderPlayer.HUD.DialogFinished += dialogFinishedHandler;
		}
	}
	public bool CanInteract(Object sender)
	{
		return sender is IsoCharacterController;
	}
}

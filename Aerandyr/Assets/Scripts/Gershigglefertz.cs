using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gershigglefertz : MonoBehaviour, IInteractable
{
	private Animator animator;
	private bool isJiggling = false;
	private float nextJiggleTime = 0.0f;
	private Dialog ourDialog;
    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
		SetNextJiggleTime();
		ourDialog = Interactions.Instance.GetDialog("Gershigglefertz");
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
			if (!senderPlayer.HUD || this.ourDialog == null)
				return;

			senderPlayer.HUD.ShowDialog(this.ourDialog);
		}
	}
	public bool CanInteract(Object sender)
	{
		return sender is IsoCharacterController;
	}
}

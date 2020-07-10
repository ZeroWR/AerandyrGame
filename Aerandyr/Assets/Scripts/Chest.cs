using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
	private Animator animator;
	private bool isOpen = false;
	private bool isInAnimation = false;
	private IsoCharacterController sender = null;
	private bool hasBeenOpened = false;
    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
    }
	public void Interact(Object sender)
	{
		this.sender = sender as IsoCharacterController;
		if (isOpen)
			Close();
		else
			Open();
	}
	public bool CanInteract(Object sender)
	{
		return sender is IsoCharacterController;
	}
	private void Open()
	{
		if (animator == null || isOpen || isInAnimation)
			return;
		isInAnimation = true;
		animator.SetBool("IsOpen", true);
	}
	private void Close()
	{
		if (animator == null || !isOpen || isInAnimation)
			return;
		isInAnimation = true;
		animator.SetBool("IsOpen", false);
	}
	public void OnOpenAnimationEnded()
	{
		isOpen = true;
		isInAnimation = false;
		if(this.sender != null)
		{
			var message = hasBeenOpened ? "This chest is empty." : "You found $20!";
			var dialog = new TransientDialog(message);
			if (hasBeenOpened)
				this.sender.HUD.ShowDialog(dialog);
			else
				this.sender.InventoryAcquiredNotification(dialog);
			this.sender = null;
		}
		this.hasBeenOpened = true;
	}
	public void OnCloseAnimationEnded()
	{
		isOpen = false;
		isInAnimation = false;
		this.sender = null;
	}
}

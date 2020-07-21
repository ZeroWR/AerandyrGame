using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
	protected Animator animator;
	protected bool isOpen = false;
	protected bool isInAnimation = false;
	protected IsoCharacterController sender = null;
	protected bool hasBeenOpened = false;
    // Start is called before the first frame update
    protected virtual void Start()
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
	public virtual bool CanInteract(Object sender)
	{
		return sender is IsoCharacterController && !isInAnimation;
	}
	private void Open()
	{
		if (animator == null || isOpen || isInAnimation)
			return;
		if (this.IsLocked)
		{
			this.sender.HUD.ShowDialog(new TransientDialog("This chest is locked."));
			return;
		}
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
	protected virtual void ChestOpened()
	{
		this.ClearSender();
	}
	protected virtual void ChestClosed()
	{
		this.ClearSender();
	}
	protected virtual bool IsLocked { get { return false; } }
	public void OnOpenAnimationEnded()
	{
		isOpen = true;
		isInAnimation = false;
		this.ChestOpened();
		this.hasBeenOpened = true;
	}
	public void OnCloseAnimationEnded()
	{
		isOpen = false;
		isInAnimation = false;
		this.ChestClosed();
	}
	protected void ClearSender()
	{
		this.sender = null;
	}
}

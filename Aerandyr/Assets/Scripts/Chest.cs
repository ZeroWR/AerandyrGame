using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
	private Animator animator;
	private bool isOpen = false;
	private bool isInAnimation = false;
    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
    }
	public void Interact(Object sender)
	{
		if (isOpen)
			Close();
		else
			Open();
	}
	public bool CanInteract(Object sender)
	{
		return true;
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
	}
	public void OnCloseAnimationEnded()
	{
		isOpen = false;
		isInAnimation = false;
	}
}

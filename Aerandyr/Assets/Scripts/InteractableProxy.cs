using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProxy : MonoBehaviour, IInteractable
{
	public GameObject ProxyTarget;
	private IInteractable interactableTarget;

	private void Awake()
	{
		//Gotta do this, because you're not allowed to expose interfaces to the editor
		interactableTarget = (ProxyTarget == null) ? null : ProxyTarget.GetComponent<IInteractable>();
		if(interactableTarget == null)
		{
			var targetName = (ProxyTarget == null) ? "NULL" : ProxyTarget.gameObject.name;
			Debug.LogError($"{this.gameObject.name} tried to set {targetName} as Target, but {targetName} does not have any components that implement IInteractable.");
			this.enabled = false;
		}
	}
	public void Interact(Object sender)
	{
		if (interactableTarget == null)
			return;
		interactableTarget.Interact(sender);
	}
	public bool CanInteract(Object sender)
	{
		return interactableTarget != null ? interactableTarget.CanInteract(sender) : false;
	}
}

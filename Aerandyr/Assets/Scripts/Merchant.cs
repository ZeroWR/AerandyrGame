using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void Interact(Object sender)
	{
		var controller = sender as IsoCharacterController;
		if (controller == null)
			return;
		this.ShowDialog(controller);
	}
	public bool CanInteract(Object sender)
	{
		return sender is IsoCharacterController;
	}

	private void ShowDialog(IsoCharacterController controller)
	{
		var ourDialog = new TransientDialog("Hello.  I'm the test merchant.", "Test Merchant", "#44c136");
		HUD.DialogFinishedEventHandler dialogFinishedHandler = null;
		dialogFinishedHandler = (s, dialog) =>
		{

			controller.HUD.DialogFinished -= dialogFinishedHandler;
		};
		controller.HUD.DialogFinished += dialogFinishedHandler;
		controller.ShowDialog(ourDialog);
	}
}

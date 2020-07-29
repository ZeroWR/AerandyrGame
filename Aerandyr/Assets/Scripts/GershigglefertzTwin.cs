using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GershigglefertzTwin : Gershigglefertz
{
	// Start is called before the first frame update
	protected override void Start()
    {
		this.dialogName = "GershigglefertzTwin";
		base.Start();
    }
	public override void Interact(Object sender)
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

		senderPlayer.ShowDialog(this.ourDialog);
		ourQuest.CurrentSection.Objectives[0].IsDone = true;
		HUD.DialogFinishedEventHandler dialogFinishedHandler = null;
		dialogFinishedHandler = (s, dialog) =>
		{
			if (dialog != ourDialog)
				return;
			ourQuest.CurrentSection = ourQuest.Sections[1];
			senderPlayer.InventoryAcquiredNotification(new TransientDialog("You received a key to Gershigglefertz's twin's chest!"));
			senderPlayer.HUD.DialogFinished -= dialogFinishedHandler;
		};
		senderPlayer.HUD.DialogFinished += dialogFinishedHandler;
	}
}

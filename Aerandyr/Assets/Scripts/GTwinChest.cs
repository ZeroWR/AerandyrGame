using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GTwinChest : Chest
{
	private Quest ourQuest = null;
	private QuestSection ourQuestSection = null;
	protected override void Start()
	{
		base.Start();
		ourQuest = QuestManager.Instance.GetQuest("Test Quest");
		if(ourQuest != null)
			ourQuestSection = ourQuest.Sections[1];
	}
	protected override bool IsLocked { get { return ourQuest == null ? true : ourQuest.CurrentSection != ourQuestSection; } }
	protected override void ChestOpened()
	{
		if (this.sender == null)
			return;
		ourQuestSection.Objectives[1].IsDone = true;
		var message = hasBeenOpened ? "This chest is empty." : "You got Gershigglefertz's item!";
		var dialog = new TransientDialog(message);
		if (hasBeenOpened)
			this.sender.HUD.ShowDialog(dialog);
		else
		{
			this.sender.InventoryAcquiredNotification(dialog).ContinueWith
				(
					(e) =>
					{
						ourQuest.CurrentSection = ourQuest.Sections[2];
						this.ClearSender();
					}
				);
		}
	}
}

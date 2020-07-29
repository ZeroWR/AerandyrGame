using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Utils;

public class GChest : Chest
{
	private Quest ourQuest = null;
	private QuestSection ourQuestSection = null;
	protected override void Start()
	{
		base.Start();
		ourQuest = QuestManager.Instance.GetQuest("Test Quest");
		if (ourQuest != null)
			ourQuestSection = ourQuest.Sections[3];
	}
	protected override bool IsLocked { get { return ourQuest == null ? true : ourQuest.CurrentSection != ourQuestSection; } }
	protected override void ChestOpened()
	{
		if (this.sender == null)
			return;

		var message = hasBeenOpened ? "This chest is empty." : "You found $20!";
		var dialog = new TransientDialog(message);
		if (hasBeenOpened)
			this.sender.ShowDialog(dialog);
		else
		{
			this.sender.InventoryAcquiredNotification(dialog).ContinueWithOnMainThread
				(
					(e) =>
					{
						ourQuestSection.Objectives[0].IsDone = true;
						this.sender.HUD.QuestCompleted(ourQuest);
						this.ClearSender();
						//TODO: Quest Completed here.
					}
				);
		}
			
	}
}

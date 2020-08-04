using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Utils;

public class GTwinChest : Chest
{
	private Quest ourQuest = null;
	private QuestSection ourQuestSection = null;
	private ColliderCallbackSource colliderCallbackSource;
	protected override void Start()
	{
		base.Start();
		ourQuest = QuestManager.Instance.GetQuest("Test Quest");
		if(ourQuest != null)
			ourQuestSection = ourQuest.Sections[1];
		colliderCallbackSource = GetComponentInChildren<ColliderCallbackSource>();
		if(colliderCallbackSource != null)
		{
			colliderCallbackSource.TriggerEnter += this.OnSeenChest;
		}
	}
	protected void OnSeenChest(GameObject sender, Collider2D ourCollider, Collider2D other)
	{
		if (ourQuestSection == null || ourQuestSection.Objectives[0].IsDone || ourQuest.CurrentSection != ourQuestSection)
			return;
		ourQuestSection.Objectives[0].IsDone = true;
		colliderCallbackSource.TriggerEnter -= this.OnSeenChest;
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
			this.sender.ShowDialog(dialog);
		else
		{
			this.sender.InventoryAcquiredNotification(dialog).ContinueWithOnMainThread
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

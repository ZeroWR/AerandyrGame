using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestLogScreen : PauseMenuScreen
{
	[SerializeField] private QuestsSelectionManager questSelectionManager = null;
	[SerializeField] private QuestDetails questDetails = null;
	[SerializeField] private QuestsController questsController = null;

	private Quest selectedQuest = null;
	public Quest SelectedQuest
	{
		get { return selectedQuest; }
		protected set
		{
			if (value == this.selectedQuest)
				return;
			this.selectedQuest = value;
			this.UpdateQuestDetails(this.selectedQuest);
		}
	}
	// Start is called before the first frame update
	protected override void Start()
    {
        if(questSelectionManager != null)
		{
			questSelectionManager.SelectedItemEvent.AddListener(this.QuestSelected);
			questSelectionManager.DeselectedItemEvent.AddListener(this.QuestDeselected);
		}
		this.UpdateQuestDetails(this.selectedQuest);
	}
	public override void Show()
	{
		base.Show();
		this.questsController.Clear();
		this.questsController.AddRange(this.controller.Quests);
	}
	protected override void Shown()
	{
		base.Shown();
		if (this.controller.Quests.Any())
			this.questSelectionManager.Select(this.controller.Quests.First());
	}
	private void QuestSelected(Quest quest)
	{
		this.SelectedQuest = quest;
	}
	private void QuestDeselected(Quest quest)
	{
		if (quest != this.selectedQuest)
			return;
		this.SelectedQuest = null;
	}
	private void UpdateQuestDetails(Quest quest)
	{
		this.questDetails.SetQuest(quest);
		this.questDetails.enabled = quest != null;
	}
}

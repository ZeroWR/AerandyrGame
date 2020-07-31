using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogScreen : PauseMenuScreen
{
	[SerializeField] private QuestsSelectionManager questSelectionManager = null;
	[SerializeField] private QuestDetails questDetails = null;
	[SerializeField] private QuestsController questsController = null;

	private Quest selectedQuest = null;
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

	// Update is called once per frame
	protected override void Update()
    {
        
    }

	public override void Show()
	{
		base.Show();
		this.questsController.Clear();
		this.questsController.AddRange(this.controller.Quests);
	}

	private void QuestSelected(Quest quest)
	{
		this.selectedQuest = quest;
		this.UpdateQuestDetails(this.selectedQuest);
	}

	private void QuestDeselected(Quest quest)
	{
		if (quest != this.selectedQuest)
			return;
		this.selectedQuest = null;
		this.UpdateQuestDetails(this.selectedQuest);
	}

	private void UpdateQuestDetails(Quest quest)
	{
		this.questDetails.SetQuest(quest);
		this.questDetails.enabled = quest != null;
	}
}

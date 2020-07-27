using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogScreen : PauseMenuScreen
{
	[SerializeField] private QuestsSelectionManager questSelectionManager = null;
	[SerializeField] private QuestDetails questDetails = null;

	private Quest selectedQuest = null;
	// Start is called before the first frame update
	protected override void Start()
    {
        if(questSelectionManager != null)
		{
			questSelectionManager.SelectedItemEvent.AddListener(this.QuestSelected);
			questSelectionManager.DeselectedItemEvent.AddListener(this.QuestDeselected);
		}
    }

	// Update is called once per frame
	protected override void Update()
    {
        
    }

	private void QuestSelected(Quest quest)
	{
		this.selectedQuest = quest;
		this.questDetails.SetQuest(quest);
	}

	private void QuestDeselected(Quest quest)
	{
		if (quest != this.selectedQuest)
			return;
		this.selectedQuest = null;
		this.questDetails.SetQuest(null);
	}
}

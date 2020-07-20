using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using OneManEscapePlan.UIList.Scripts;

public class QuestObjectivesController : UIListController<QuestObjective, QuestObjectiveView>
{
	public IsoCharacterController Controller { get; set; }
	private QuestSection currentlyShowingSection = null;
	private QuestSection CurrentlyShowingSection
	{
		get { return currentlyShowingSection; }
		set
		{
			Clear();
			currentlyShowingSection = value;
			if (currentlyShowingSection != null)
				UpdateItems();
		}
	}

	private void Update()
	{
		if (Controller == null)
		{
			if (this.itemModels.Any())
				this.Clear();
		}
		else
		{
			if (Controller.CurrentQuest != null)
			{
				if(this.currentlyShowingSection != Controller.CurrentQuest.CurrentSection)
				{
					this.CurrentlyShowingSection = Controller.CurrentQuest.CurrentSection;
				}
			}
			else if(CurrentlyShowingSection != null)
			{
				CurrentlyShowingSection = null; //CurrentQuest is null, so set the CurrentlyShowingSection to null;
			}
		}
	}
	private void UpdateItems()
	{
		if (CurrentlyShowingSection == null)
			return;
		LayoutGroup layout = GetComponent<LayoutGroup>();
		if(layout != null)
		{
			if(layout is VerticalLayoutGroup)
			{
				var vLayout = layout as VerticalLayoutGroup;
			}
		}
		this.AddRange(CurrentlyShowingSection.Objectives);
	}
}

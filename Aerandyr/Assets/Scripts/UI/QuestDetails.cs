using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetails : MonoBehaviour
{
	[SerializeField]
	private Text QuestTitle;
	[SerializeField]
	private Text QuestDescription;

	public void SetQuest(Quest quest)
	{
		QuestTitle.text = quest != null ? quest.Name : string.Empty;
		QuestDescription.text = quest != null ? quest.Description : string.Empty;
	}
}

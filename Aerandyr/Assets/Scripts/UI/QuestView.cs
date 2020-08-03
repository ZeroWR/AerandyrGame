using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using OneManEscapePlan.UIList.Scripts;

public class QuestView : UIListItemViewBase<Quest>
{
	[SerializeField] protected Text questNameText = null;
	private void Start()
	{
		questNameText = GetComponentInChildren<Text>();
	}
	public override void Refresh()
	{
		if (questNameText.text != model.Name)
			questNameText.text = model.Name;
	}
	private void Update()
	{
		Refresh();
	}
}

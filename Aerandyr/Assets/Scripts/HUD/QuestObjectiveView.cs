﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OneManEscapePlan.UIList.Scripts;

[RequireComponent(typeof(Toggle))]
public class QuestObjectiveView : UIListItemViewBase<QuestObjective>
{
	[SerializeField] protected Toggle toggleField = null;
	[SerializeField] protected Text toggleText = null;
	private void Start()
	{
		if(toggleField == null)
		{
			toggleField = GetComponent<Toggle>();
			if (toggleField != null)
			{
				var colors = toggleField.colors;
				colors.disabledColor = colors.normalColor;
				toggleField.colors = colors;
				toggleField.interactable = false;
			}
		}
		toggleText = GetComponentInChildren<Text>();
	}
	public override void Refresh()
	{
		toggleText.text = model.Name;
		toggleField.isOn = model.IsDone;
	}
}

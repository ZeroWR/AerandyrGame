using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using OneManEscapePlan.UIList.Scripts;

public class InventorySlotView : UIListItemViewBase<InventoryItem>
{
	[SerializeField] protected Text quantityText = null;
	[SerializeField] protected Image itemImage = null;
	public override void Refresh()
	{
		if (quantityText.text != model.Quantity.ToString())
			quantityText.text = model.Quantity.ToString();
		if (itemImage.sprite != model.ItemDefintion.Sprite)
			itemImage.sprite = model.ItemDefintion.Sprite;
	}
	private void Update()
	{
		Refresh();
	}
}

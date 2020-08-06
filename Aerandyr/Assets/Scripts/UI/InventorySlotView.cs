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
	[SerializeField] protected Image borderImage = null;
	public Color SelectionColor = Color.white;
	private Color defaultBorderColor;
	private void Start()
	{
		this.defaultBorderColor = borderImage.color;
	}
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
	public override bool IsSelected
	{
		get => base.IsSelected;
		set
		{
			base.IsSelected = value;
			if(this.borderImage)
			{
				this.borderImage.color = value ? this.SelectionColor : this.defaultBorderColor;
			}
		}
	}
}

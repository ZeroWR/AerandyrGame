using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneManEscapePlan.UIList.Scripts;
using UnityEngine;

public class InventoryScreen : PauseMenuScreen
{
	[SerializeField] private InventorySlotSelectionManager inventorySlotSelectionManager = null;
	[SerializeField] private InventoryItemDetails inventoryItemDetails = null;
	[SerializeField] private InventoryItemsController inventoryItemsController = null;
	private InventoryItem selectedItem = null;
	protected override void Start()
	{
		if (inventorySlotSelectionManager != null)
		{
			inventorySlotSelectionManager.SelectedItemEvent.AddListener(this.ItemSelected);
			inventorySlotSelectionManager.DeselectedItemEvent.AddListener(this.ItemDeselected);
		}
		this.UpdateItemDetails(this.selectedItem);
	}
	public override void Show()
	{
		base.Show();
		this.inventoryItemsController.Clear();
		this.inventoryItemsController.AddRange(this.controller.Inventory);
		this.UpdateItemDetails(this.selectedItem);
	}

	private void ItemSelected(InventoryItem inventoryItem)
	{
		this.selectedItem = inventoryItem;
		this.UpdateItemDetails(this.selectedItem);
	}

	private void ItemDeselected(InventoryItem inventoryItem)
	{
		if (inventoryItem != this.selectedItem)
			return;
		this.selectedItem = null;
		this.UpdateItemDetails(this.selectedItem);
	}

	private void UpdateItemDetails(InventoryItem inventoryItem)
	{
		this.inventoryItemDetails.SetItem(inventoryItem);
		this.inventoryItemDetails.enabled = inventoryItem != null;
	}
}

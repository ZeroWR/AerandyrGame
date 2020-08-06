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
	public InventoryItem SelectedItem
	{
		get { return selectedItem; }
		protected set
		{
			if (value == this.selectedItem)
				return;
			this.selectedItem = value;
			this.UpdateItemDetails(this.selectedItem);
		}
	}
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
	}
	protected override void Shown()
	{
		base.Shown();
		if (this.controller.Inventory.Any())
			this.inventorySlotSelectionManager.Select(this.controller.Inventory.First());
	}
	private void ItemSelected(InventoryItem inventoryItem)
	{
		this.SelectedItem = inventoryItem;
	}
	private void ItemDeselected(InventoryItem inventoryItem)
	{
		if (inventoryItem != this.selectedItem)
			return;
		this.SelectedItem = null;
	}
	private void UpdateItemDetails(InventoryItem inventoryItem)
	{
		this.inventoryItemDetails.SetItem(inventoryItem);
		this.inventoryItemDetails.enabled = inventoryItem != null;
	}
	public override void ProcessInput()
	{
		if (!this.controller.Inventory.Any())
			return;

		var collection = this.inventoryItemsController.Data.ToList();

		InventoryItem itemToSelect = null;
		if(selectedItem == null)
		{
			itemToSelect = this.controller.Inventory.First();
		}
		else
		{
			//Look at this mess.
			float horizontalInput = Input.GetAxis("Horizontal");
			float verticalInput = Input.GetAxis("Vertical");
			Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
			inputVector = Vector2.ClampMagnitude(inputVector, 1);
			if (inputVector.x == 0 && inputVector.y == 0)
				return;
			var currentIndex = collection.IndexOf(this.selectedItem);
			var columns = this.inventoryItemsController.Columns;
			var rows = this.inventoryItemsController.Rows;
			var nextSpot = currentIndex;
			if(inputVector.y != 0.0f)
			{
				var currentRow = currentIndex / columns;
				int verticalDirection = 1 * -((int)(inputVector.y / Math.Abs(inputVector.y)));
				var spotDifference = (this.inventoryItemsController.Columns * verticalDirection);
				var projectedSpot = currentIndex + spotDifference;
				if (projectedSpot < 0 || projectedSpot > (this.controller.Inventory.Count - 1))
				{
					spotDifference = 0;
				}
				else
				{
					var projectedRow = projectedSpot / columns;
					var rowDifference = currentRow - projectedRow;
					if (Math.Abs(rowDifference) > 1)
						spotDifference = 0;
				}
				nextSpot += spotDifference;
			}
			if (inputVector.x != 0.0f)
			{
				var currentColumn = currentIndex % columns;
				int horizontalDirection = 1 * (int)(inputVector.x / Math.Abs(inputVector.x));
				var spotDifference = horizontalDirection;
				var projectedSpotInColumn = currentColumn + spotDifference;
				var projectedIndex = nextSpot + spotDifference;
				if (projectedSpotInColumn < 0 || projectedSpotInColumn > (columns - 1) || collection.Count <= projectedIndex)
					spotDifference = 0;
				nextSpot += spotDifference;
			}

			itemToSelect = collection.ElementAt(nextSpot);
		}

		if(itemToSelect != null)
		{
			this.inventorySlotSelectionManager.Select(itemToSelect);
			SetNextInputTime(0.25f);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class InventoryItemDetails : MonoBehaviour
{
	[SerializeField]
	private Text ItemTitle;
	[SerializeField]
	private Text ItemDescription;

	public void SetItem(InventoryItem inventoryItem)
	{
		ItemTitle.text = inventoryItem != null ? inventoryItem.ItemDefintion.Name : string.Empty;
		ItemDescription.text = inventoryItem != null ? inventoryItem.ItemDefintion.Description : string.Empty;
	}
}
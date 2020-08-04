using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneManEscapePlan.UIList.Scripts;
using UnityEngine;

[RequireComponent(typeof(InventoryItemsController))]
public class InventorySlotSelectionManager : UIListSelectionManagerBase<InventoryItem, InventorySlotView>
{
}

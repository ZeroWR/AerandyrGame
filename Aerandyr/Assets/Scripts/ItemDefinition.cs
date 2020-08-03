using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ItemCategory
{
	Resource,
	Weapon,
	Armor,
	Shield,
	Quest,
	Inventory,
	Money
}

[Serializable]
public class ItemDefinition
{
	public int ID;
	public string Name;
	public string Description;
	public ItemCategory ItemCategory;
	public int MaxCarry;
	public int MaxPerSlot;
	public Sprite Sprite;
}

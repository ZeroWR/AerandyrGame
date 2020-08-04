using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : MonoBehaviour
{
	public ItemDefinition ItemDefinition;
	public int Quantity;

	private void Start()
	{
		GetComponent<SpriteRenderer>().sprite = ItemDefinition.Sprite;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var controller = collision.gameObject.GetComponent<IsoCharacterController>();
		if (controller == null)
			return;
		if (!controller.CanPickUpItem(this.ItemDefinition, this.Quantity))
			return;

		controller.PickupItem(this.ItemDefinition, this.Quantity);
		Destroy(this.gameObject);
	}
}

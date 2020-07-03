using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageTrigger : MonoBehaviour
{
	private List<GameObject> touchingDamageables = new List<GameObject>();
	// Start is called before the first frame update
	void Start()
    {
        
    }
	public void DoDamage(GameObject sender, int damage, Vector2 fromPosition)
	{
		foreach(var obj in touchingDamageables)
		{
			var damageable = obj.GetComponent<ICanTakeDamage>();
			if (damageable == null)
				continue;
			Vector2 force = (sender.transform.position - obj.transform.position) * -500000;
			damageable.TakeDamage(sender, damage, force);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var damageable = collision.gameObject.GetComponent<ICanTakeDamage>();
		if (damageable == null || touchingDamageables.Contains(collision.gameObject))
			return;
		touchingDamageables.Add(collision.gameObject);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		var damageable = collision.gameObject.GetComponent<ICanTakeDamage>();
		if (damageable == null || !touchingDamageables.Contains(collision.gameObject))
			return;
		touchingDamageables.Remove(collision.gameObject);
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageTrigger : MonoBehaviour
{
	protected List<GameObject> touchingDamageables = new List<GameObject>();
	// Start is called before the first frame update
	void Start()
    {
        
    }
	public virtual void DoDamageToAll(GameObject sender, int damage, Vector2 fromPosition)
	{
		try
		{
			foreach (var obj in touchingDamageables)
			{
				this.DoDamage(sender, obj, damage, fromPosition);
			}
		}
		catch(Exception ex)
		{
			var stopHere = ex.Message;
			Debug.Log("You should have stopped here.");
			Debug.LogError(ex);
		}
	}
	public virtual void DoDamage(GameObject sender, GameObject target, int damage, Vector2 fromPosition)
	{
		var damageable = target.GetComponent<ICanTakeDamage>();
		if (damageable == null)
			return;
		Vector2 force = (sender.transform.position - target.transform.position) * -500000;
		damageable.TakeDamage(sender, damage, force);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var damageable = collision.gameObject.GetComponent<ICanTakeDamage>();
		if (damageable == null || touchingDamageables.Contains(collision.gameObject))
			return;
		this.OnDamageableEntered(collision.gameObject);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		var damageable = collision.gameObject.GetComponent<ICanTakeDamage>();
		if (damageable == null || !touchingDamageables.Contains(collision.gameObject))
			return;
		this.OnDamageableExited(collision.gameObject);
	}
	protected virtual void OnDamageableEntered(GameObject gameObject)
	{
		touchingDamageables.Add(gameObject);
	}
	protected virtual void OnDamageableExited(GameObject gameObject)
	{
		touchingDamageables.Remove(gameObject);
	}
}

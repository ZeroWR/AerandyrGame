using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageTrigger : MonoBehaviour
{
	protected List<GameObject> touchingDamageables = new List<GameObject>();
	protected float defaultForceMagnitude = 50.0f;
	protected Vector2 defaultForceMagnitudeVector;
	// Start is called before the first frame update
	void Start()
    {
		defaultForceMagnitudeVector = new Vector2(defaultForceMagnitude, defaultForceMagnitude);
	}
	public virtual void DoDamageToAll(GameObject sender, int damage, Vector2 fromPosition)
	{
		DoDamageToAll(sender, damage, fromPosition, defaultForceMagnitudeVector);
	}
	public virtual void DoDamageToAll(GameObject sender, int damage, Vector2 fromPosition, Vector2 forceMagnitude)
	{
		try
		{
			//Create a copy, because some things that are damaged turn off their collider, which removes them from the
			//list of touchingDamageables right away.  This messes up our "foreach" loop.
			var copyOfTouchingDamageables = new List<GameObject>(touchingDamageables);
			foreach (var obj in copyOfTouchingDamageables)
			{
				this.DoDamage(sender, obj, damage, fromPosition, forceMagnitude);
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
		DoDamage(sender, target, damage, fromPosition, defaultForceMagnitudeVector);
	}
	public virtual void DoDamage(GameObject sender, GameObject target, int damage, Vector2 fromPosition, Vector2 forceMagnitude)
	{
		var damageable = target.GetComponent<ICanTakeDamage>();
		if (damageable == null)
			return;
		Vector2 direction = (sender.transform.position - target.transform.position);
		direction.Normalize();
		direction = -direction;
		Vector2 force = direction * forceMagnitude;
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

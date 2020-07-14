using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoDamageTrigger : MonoBehaviour
{
	protected List<Collider2D> touchingDamageables = new List<Collider2D>();
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
			var copyOfTouchingDamageables = new List<Collider2D>(touchingDamageables);
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
	public virtual void DoDamage(GameObject sender, Collider2D target, int damage, Vector2 fromPosition)
	{
		DoDamage(sender, target, damage, fromPosition, defaultForceMagnitudeVector);
	}
	public virtual void DoDamage(GameObject sender, Collider2D target, int damage, Vector2 fromPosition, Vector2 forceMagnitude)
	{
		var damageable = target.GetComponent<ICanTakeDamage>();
		if (damageable == null)
			return;
		Vector2 direction = (sender.transform.position - target.transform.position);
		Vector2 directionNormalized = direction;
		if(directionNormalized.magnitude != 1)
			directionNormalized.Normalize();
		Vector2 oppositeDirection = -directionNormalized;
		Vector2 force = oppositeDirection * forceMagnitude;
		//var debugMessage = string.Format("sender: {0}\nsender position: {1}\ntarget: {2}\ntarget position: {3}\ndirection: {4}\ndirection normalized: {5}\ndirection normalized reversed: {6}\nforce: {7}\ntarget's gameObject position: {8}",
		//	sender.name,
		//	sender.transform.position.ToString(),
		//	target.name,
		//	target.transform.position.ToString(),
		//	direction.ToString(),
		//	directionNormalized.ToString(),
		//	oppositeDirection.ToString(),
		//	force.ToString(),
		//	target.gameObject.transform.position.ToString());
		//Debug.Log(string.Format("=====DoDamage START=====\n{0}\n=====DoDamage END=====", debugMessage));
		damageable.TakeDamage(sender, damage, force);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var damageable = collision.gameObject.GetComponent<ICanTakeDamage>();
		if (damageable == null || touchingDamageables.Any(x => x.gameObject == collision.gameObject))
			return;
		this.OnDamageableEntered(collision);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		var damageable = collision.gameObject.GetComponent<ICanTakeDamage>();
		var existing = touchingDamageables.SingleOrDefault(x => x.gameObject == collision.gameObject);
		if (damageable == null || existing == null)
			return;
		this.OnDamageableExited(existing);
	}
	protected virtual void OnDamageableEntered(Collider2D container)
	{
		touchingDamageables.Add(container);
	}
	protected virtual void OnDamageableExited(Collider2D container)
	{
		touchingDamageables.Remove(container);
	}
}

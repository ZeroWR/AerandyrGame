using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ColliderCallbackSource : MonoBehaviour
{
	public Collider2D TargetCollider;

	private void Start()
	{
		if(!TargetCollider)
		{
			TargetCollider = GetComponent<Collider2D>();
		}
	}


	public delegate void CollisionCallbackHandler(GameObject sender, Collider2D collider, Collision2D collision);
	public delegate void TriggerCallbackHandler(GameObject sender, Collider2D ourCollider, Collider2D otherCollider);

	public TriggerCallbackHandler TriggerEnter;
	public TriggerCallbackHandler TriggerStay;
	public TriggerCallbackHandler TriggerExit;

	// Start is called before the first frame update
	private void OnTriggerEnter2D(Collider2D collision)
	{
		TriggerEnter?.Invoke(this.gameObject, TargetCollider, collision);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		TriggerExit?.Invoke(this.gameObject, TargetCollider, collision);
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		TriggerStay?.Invoke(this.gameObject, TargetCollider, collision);
	}

	public CollisionCallbackHandler CollisionEnter;
	public CollisionCallbackHandler CollisionStay;
	public CollisionCallbackHandler CollisionExit;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		CollisionEnter?.Invoke(this.gameObject, TargetCollider, collision);
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		CollisionStay?.Invoke(this.gameObject, TargetCollider, collision);
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		CollisionExit?.Invoke(this.gameObject, TargetCollider, collision);
	}
}

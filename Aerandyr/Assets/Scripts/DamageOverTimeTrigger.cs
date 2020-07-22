using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeContainer
{
	private Collider2D target = null;
	private DamageOverTimeTrigger sender = null;
	private Coroutine coroutine = null;
	private bool enabled = false;
	public DamageOverTimeContainer(Collider2D target, DamageOverTimeTrigger sender)
	{
		this.target = target;
		this.sender = sender;
	}
	public Collider2D Target { get { return target; } }
	public void Start()
	{
		if (this.coroutine != null)
			return;
		this.enabled = true;
		this.coroutine = this.sender.StartCoroutine(this.DoDamage());
	}
	public void Stop()
	{
		if (this.coroutine == null)
			return;
		this.enabled = false;
		this.sender.StopCoroutine(this.coroutine);
		this.coroutine = null;
	}
	protected IEnumerator DoDamage()
	{
		while(enabled)
		{
			var collider = this.sender.GetComponent<Collider2D>();
			var fromPosition = collider != null ? collider.transform.position : this.sender.transform.position;
			this.sender.DoDamage(this.sender.gameObject, this.target, this.sender.Damage, fromPosition);
			yield return new WaitForSeconds(this.sender.EveryXSeconds);
		}
	}
}

public class DamageOverTimeTrigger : DoDamageTrigger
{
	public int Damage = 1;
	public int EveryXSeconds = 1;
	private List<DamageOverTimeContainer> damageContainers = new List<DamageOverTimeContainer>();

	protected override void OnDamageableEntered(Collider2D gameObject)
	{
		base.OnDamageableEntered(gameObject);
		var damageContainer = new DamageOverTimeContainer(gameObject, this);
		damageContainer.Start();
		this.damageContainers.Add(damageContainer);
	}
	protected override void OnDamageableExited(Collider2D gameObject)
	{
		base.OnDamageableExited(gameObject);
		var damageContainer = this.damageContainers.Find(x => x.Target == gameObject);
		if (damageContainer == null)
			return;
		damageContainer.Stop();
		this.damageContainers.Remove(damageContainer);
	}
}

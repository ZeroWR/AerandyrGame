using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ICanTakeDamage
{
	public int Health = 100;
	public int MaxHealth = 100;
	private CharacterAnimationController characterAnimationController;
	private bool isDead = false;
	public bool IsDead { get { return isDead; } }
	// Start is called before the first frame update
	protected virtual void Start()
    {
		characterAnimationController = GetComponent<CharacterAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public virtual void TakeDamage(GameObject sender, int damage, Vector2 force)
	{
		if (this.isDead)
			return;
		this.Health -= damage;
		if(this.Health <= 0)
			Die();
	}
	protected virtual void Die()
	{
		if (this.isDead)
			return;
		if(this.characterAnimationController != null)
			this.characterAnimationController.ChangeParameter("IsDead", true);
		this.isDead = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ICanTakeDamage
{
	public int Health = 100;
	private CharacterAnimationController characterAnimationController;
	// Start is called before the first frame update
	void Start()
    {
		characterAnimationController = GetComponent<CharacterAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void TakeDamage(GameObject sender, int damage, Vector2 force)
	{
		this.Health -= damage;
	}
}

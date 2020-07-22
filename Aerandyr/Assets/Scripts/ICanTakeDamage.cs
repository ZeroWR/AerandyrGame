using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanTakeDamage
{
	//Should probably add a damage type
	void TakeDamage(GameObject sender, int damage, Vector2 force);
}

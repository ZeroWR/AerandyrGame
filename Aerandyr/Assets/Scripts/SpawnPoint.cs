using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public string Name = "Default";

	public readonly static string DefaultName = "Default";

	private void Awake()
	{
		var sprites = GetComponentsInParent<SpriteRenderer>();
		foreach(var sprite in sprites)
		{
			sprite.enabled = false;
		}
	}
}

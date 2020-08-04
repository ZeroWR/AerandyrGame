using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public string Name = "Default";

	public readonly static string DefaultName = "Default";

	public SpriteRenderer Sprite;

	private void Awake()
	{
		if(Sprite != null)
		{
			Sprite.enabled = false;
		}
	}
}

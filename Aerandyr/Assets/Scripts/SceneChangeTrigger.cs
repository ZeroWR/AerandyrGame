using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class SceneChangeTrigger : MonoBehaviour
{
	public string SceneToChangeTo = string.Empty;
	public string TargetSpawnPoint = SpawnPoint.DefaultName;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var playerController = collision.GetComponent<IsoCharacterController>();
		if (playerController == null)
			return;

		if(string.IsNullOrEmpty(SceneToChangeTo))
		{
			Debug.LogError("LevelChangeTrigger without LevelToChangeTo!");
			return;
		}

		SceneTransitionManager.Instance.LoadScene(playerController.gameObject, SceneToChangeTo, TargetSpawnPoint);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	private static SceneTransitionManager instance = null;
	public static SceneTransitionManager Instance { get { return instance; } }

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void LoadScene(GameObject player, string sceneName, string spawnPoint = "Default")
	{
		var currentScene = SceneManager.GetActiveScene();
		UnityEngine.Events.UnityAction<Scene, LoadSceneMode> callback = null;
		callback = (scene, loadSceneMode) =>
		{
			SceneManager.sceneLoaded -= callback;
			SceneManager.MoveGameObjectToScene(player, scene);
			SceneManager.UnloadSceneAsync(currentScene);
			var spawnPoints = FindObjectsOfType<SpawnPoint>();
			if(spawnPoints.Length <= 0)
			{
				Debug.LogError($"No spawn points in level {sceneName}");
				return;
			}
			var targetSpawnPoint = spawnPoints.FirstOrDefault(x => x.Name == spawnPoint);
			var spawnPointToSpawnAt = targetSpawnPoint != null ? targetSpawnPoint : spawnPoints.First();
			Debug.Log($"Spawning at spawn point {spawnPointToSpawnAt.Name} at position {spawnPointToSpawnAt.transform.position.ToString()}");
			player.transform.position = spawnPointToSpawnAt.transform.position;

			var cameras = FindObjectsOfType<BasicCameraFollow>();
			if (cameras.Length != 0)
			{
				foreach(var camera in cameras)
				{
					camera.followTarget = player;
					camera.SnapToTarget();
				}
			}

			var characters = FindObjectsOfType<IsoCharacterController>();
			Debug.Log($"Found {characters.Length} characters.");
			var destroyCharacters = characters.Where(x => x.gameObject != player);
			Debug.Log($"Found {destroyCharacters.Count()} characters to destroy.");
			foreach (var destroyCharacter in destroyCharacters)
			{
				Debug.Log($"Destroying character with controller id {destroyCharacter.ControllerId}");
				Destroy(destroyCharacter.gameObject);
			}
		};
		var currentPlayerController = player.GetComponent<IsoCharacterController>();
		Debug.Log($"Loading scene {sceneName} and spawning at {spawnPoint} for controller {currentPlayerController.ControllerId}");
		SceneManager.sceneLoaded += callback;
		SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
	}
}

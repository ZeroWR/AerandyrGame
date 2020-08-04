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

	public void LoadScene(GameObject player, string sceneName, string spawnPoint = "Default", SceneTransition transitionToGoBackTo = null)
	{
		var currentScene = SceneManager.GetActiveScene();
		UnityEngine.Events.UnityAction<Scene, LoadSceneMode> callback = null;
		bool setTransition = transitionToGoBackTo != null;
		string goBackToSpawnPoint = null, goBackToScene = null;
		if (setTransition)
		{
			goBackToSpawnPoint = transitionToGoBackTo.SpawnPoint.Name;
			goBackToScene = currentScene.name;
		}
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
			player.transform.position = spawnPointToSpawnAt.transform.position;

			if(setTransition)
			{
				var currentTransition = targetSpawnPoint.GetComponentInParent<SceneTransition>();
				if(currentTransition != null)
				{
					currentTransition.SceneChangeTrigger.SceneToChangeTo = goBackToScene;
					currentTransition.SceneChangeTrigger.TargetSpawnPoint = goBackToSpawnPoint;
				}
			}

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
			var destroyCharacters = characters.Where(x => x.gameObject != player);
			foreach (var destroyCharacter in destroyCharacters)
			{
				Destroy(destroyCharacter.gameObject);
			}
		};
		SceneManager.sceneLoaded += callback;
		SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
	}
}

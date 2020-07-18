using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	private static QuestManager instance = null;
	public static QuestManager Instance { get { return instance; } }

	public List<Quest> Quests { get; protected set; }
	public string QuestsDirectory = "Quests";
	public QuestManager()
	{
		this.Quests = new List<Quest>();
	}
	// Start is called before the first frame update
	void Start()
    {
		this.LoadQuests();
    }
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
	private void LoadQuests()
	{
		var questScripts = Resources.LoadAll<TextAsset>(QuestsDirectory);
		foreach (var questScript in questScripts)
		{
			Quest quest = JsonUtility.FromJson<Quest>(questScript.text);
			if (quest == null)
				continue;
			if (string.IsNullOrEmpty(quest.Name))
				continue;

			Quests.Add(quest);
		}
	}
}

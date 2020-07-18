using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Interactions : MonoBehaviour
{
    // Use this for initialization
    private static Interactions interactions = null;
	public static Interactions Instance { get { return interactions; } }

    private Dictionary<string, Dialog> Dialogs;
    private Dictionary<string, Sprite> DialogSpriteCache;

    public string DialogScriptsDirectory = "Dialog";

    public Interactions()
    {
        Dialogs = new Dictionary<string, Dialog>();
        DialogSpriteCache = new Dictionary<string, Sprite>();
    }
    void Start()
    {
        LoadDialogs();
        CacheDialogSprites();
    }
	public Dialog GetDialog(string dialogName)
	{
		Dialog rtn = null;
		this.Dialogs.TryGetValue(dialogName, out rtn);
		return rtn;
	}
    private void LoadDialogs()
    {
        var dialogScripts = Resources.LoadAll<TextAsset>(DialogScriptsDirectory);
        foreach (var dialogScript in dialogScripts)
        {
            Dialog dialog = JsonUtility.FromJson<Dialog>(dialogScript.text);
            if (dialog == null)
                continue;
            if (string.IsNullOrEmpty(dialog.Name))
                continue;

            Dialogs.Add(dialog.Name, dialog);
        }
    }
    private void CacheDialogSprites()
    {
        HashSet<string> spritePaths = new HashSet<string>(Dialogs.SelectMany(x => from section in x.Value.Sections where !string.IsNullOrEmpty(section.Icon) select section.Icon));
        foreach (var path in spritePaths)
        {
            Sprite loadedSprite = Resources.Load<Sprite>(path);
            if (loadedSprite != null)
                DialogSpriteCache.Add(path, loadedSprite);
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (interactions == null)
        {
            interactions = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}


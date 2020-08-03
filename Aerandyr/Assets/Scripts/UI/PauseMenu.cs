using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public PauseMenuScreen[] ScreensDefinitions;
	private List<PauseMenuScreen> screens = new List<PauseMenuScreen>();
	public Canvas ScreenContainer;
	public Text ScreenTitle;
	protected IsoCharacterController controller;
	public IsoCharacterController Controller
	{
		get { return controller; }
		set
		{
			this.controller = value;
			foreach(var screen in screens)
			{
				screen.SetController(this.controller);
			}
		}
	}
	private PauseMenuScreen activeScreen = null;
	private void Awake()
	{
		var screenContainerRect = this.ScreenContainer.GetComponent<RectTransform>();
		foreach (var screenDefinition in ScreensDefinitions)
		{
			var newScreen = Instantiate(screenDefinition);
			var rectTransform = newScreen.GetComponent<RectTransform>();
			newScreen.transform.parent = this.ScreenContainer.transform;
			if (rectTransform)
			{
				//rectTransform.SetParent(this.ScreenContainer.transform);
				//rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
				//rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
				//rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
				//rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(1, 1);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.sizeDelta = Vector2.zero;
				rectTransform.localPosition = Vector3.zero;
				rectTransform.position = Vector3.zero;
				rectTransform.anchoredPosition = Vector2.zero;
			}
			newScreen.enabled = false;
			newScreen.SetController(this.controller);
			screens.Add(newScreen);
		}
	}
	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Toggle(string screenToToggleTitle = null)
	{
		bool hide = this.enabled;
		if(!string.IsNullOrEmpty(screenToToggleTitle) && this.activeScreen != null)
		{
			hide = this.activeScreen.Title == screenToToggleTitle;
		}
		if (hide)
			this.Hide();
		else
			this.Show(screenToToggleTitle);
	}
	public void Show(string screenToShowTitle = null)
	{
		this.gameObject.SetActive(true);
		this.enabled = true;
		if (screens.Count > 0)
		{
			var screenToShow = string.IsNullOrEmpty(screenToShowTitle) ? screens.First() : screens.Find(x => x.Title == screenToShowTitle);
			if(screenToShow == null)
				screenToShow = screens.First();
			if(this.activeScreen != null && screenToShow != this.activeScreen)
			{
				this.activeScreen.Hide();
			}
			this.activeScreen = screenToShow;
			screenToShow.Show();
			if(this.ScreenTitle != null)
			{
				this.ScreenTitle.text = screenToShow.Title;
			}

			var allOtherScreens = this.screens.Where(x => x != screenToShow);
			foreach(var screen in allOtherScreens)
			{
				screen.Hide();
			}
		}
	}
	public void Hide()
	{
		this.enabled = false;
		this.gameObject.SetActive(false);
		if(this.activeScreen != null)
		{
			this.activeScreen.Hide();
			this.activeScreen = null;
		}
	}
}

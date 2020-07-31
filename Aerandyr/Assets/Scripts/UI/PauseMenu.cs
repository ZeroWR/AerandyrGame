using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public PauseMenuScreen[] ScreensDefinitions;
	private List<PauseMenuScreen> screens = new List<PauseMenuScreen>();
	public Canvas ScreenContainer;
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
		if(screens.Count > 0)
		{
			screens.First().enabled = true;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Toggle()
	{
		if (this.enabled)
			this.Hide();
		else
			this.Show();
	}
	public void Show()
	{
		this.gameObject.SetActive(true);
		this.enabled = true;
		if (screens.Count > 0)
		{
			screens.First().Show();
		}
	}
	public void Hide()
	{
		this.enabled = false;
		this.gameObject.SetActive(false);
	}
}

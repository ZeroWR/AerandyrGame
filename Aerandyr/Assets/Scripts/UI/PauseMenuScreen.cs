using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShowOrHiddenUpdate
{
	Shown,
	Hidden
}

public class PauseMenuScreen : MonoBehaviour
{
	public string Title;
	protected IsoCharacterController controller;
	private Nullable<ShowOrHiddenUpdate> shownOrHiddenUpdate = null;
	private float nextUIInputTime = 0.0f;
	// Start is called before the first frame update
	protected virtual void Start()
    {
        
    }

	// Update is called once per frame
	protected virtual void Update()
    {
        if(shownOrHiddenUpdate != null)
		{
			if(shownOrHiddenUpdate == ShowOrHiddenUpdate.Shown)
			{
				Shown();
			}
			else
			{
				Hidden();
			}
			shownOrHiddenUpdate = null;
		}
    }
	public virtual void SetController(IsoCharacterController controller)
	{
		this.controller = controller;
	}
	public virtual void Show()
	{
		this.gameObject.SetActive(true);
		this.enabled = true;
		this.shownOrHiddenUpdate = ShowOrHiddenUpdate.Shown;
		this.nextUIInputTime = -1;
	}
	public virtual void Hide()
	{
		this.gameObject.SetActive(false);
		this.enabled = false;
		this.shownOrHiddenUpdate = ShowOrHiddenUpdate.Hidden;
		this.nextUIInputTime = -1;
	}
	protected virtual void Shown()
	{
		SetNextInputTime(0);
	}
	protected virtual void Hidden()
	{
		SetNextInputTime(0);
	}
	public virtual void ProcessInput() { }
	public bool CanProcessInput
	{
		get { return this.nextUIInputTime <= Time.time && this.nextUIInputTime >= 0; }
	}
	protected void SetNextInputTime(float millisecondsFromNow)
	{
		this.nextUIInputTime = Time.time + millisecondsFromNow;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScreen : MonoBehaviour
{
	public string Title;
	protected IsoCharacterController controller;
	// Start is called before the first frame update
	protected virtual void Start()
    {
        
    }

	// Update is called once per frame
	protected virtual void Update()
    {
        
    }
	public virtual void SetController(IsoCharacterController controller)
	{
		this.controller = controller;
	}
	public virtual void Show()
	{
		this.gameObject.SetActive(true);
		this.enabled = true;
	}
	public virtual void Hide()
	{
		this.gameObject.SetActive(false);
		this.enabled = false;
	}
}

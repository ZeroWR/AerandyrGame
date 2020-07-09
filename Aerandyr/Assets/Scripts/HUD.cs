using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public Text HealthText;
	public Canvas DialogPanel;
	public Text DialogText;
	public Player Player { get; set; }
	private float nextKeyDownTime = 0.0f;
	public float KeyPressInterval = 0.5f;
	// Start is called before the first frame update
	void Start()
    {
		this.DialogPanel.enabled = false;
		this.DialogText.text = string.Empty;
    }
    // Update is called once per frame
    void Update()
    {
		UpdateHealthText();
    }
	public void ProcessInput()
	{
		if (this.IsInDialog && this.CanProcessKeyPress && Input.GetKeyDown(KeyCode.E))
		{
			CloseDialog();
		}
	}
	private void UpdateHealthText()
	{
		if (this.HealthText != null && this.Player != null)
		{
			this.HealthText.text = this.Player.Health.ToString();
		}
	}
	public void ShowDialog(string dialogText)
	{
		this.DialogText.text = dialogText;
		this.DialogPanel.enabled = true;
		this.nextKeyDownTime = Time.time + this.KeyPressInterval;
	}
	public void CloseDialog()
	{
		this.DialogText.text = string.Empty;
		this.DialogPanel.enabled = false;
	}
	public bool IsInDialog { get { return this.DialogPanel != null && this.DialogPanel.enabled; } }
	private bool CanProcessKeyPress { get { return Time.time >= this.nextKeyDownTime; } }
}

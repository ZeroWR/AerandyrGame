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
		if(this.DialogPanel.enabled && Input.GetKeyDown(KeyCode.E))
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
	}
	public void CloseDialog()
	{
		this.DialogText.text = string.Empty;
		this.DialogPanel.enabled = false;
	}
}

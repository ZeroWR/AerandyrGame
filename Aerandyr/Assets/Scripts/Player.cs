using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
	public Text HealthText;
    // Start is called before the first frame update
    protected override void Start()
    {
		UpdateHealthText();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public override void TakeDamage(GameObject sender, int damage, Vector2 force)
	{
		base.TakeDamage(sender, damage, force);
		UpdateHealthText();
	}
	private void UpdateHealthText()
	{
		if (this.HealthText != null)
		{
			this.HealthText.text = this.Health.ToString();
		}
	}
}

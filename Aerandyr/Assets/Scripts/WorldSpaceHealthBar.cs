using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceHealthBar : MonoBehaviour
{
	public Canvas HealthCanvas = null;
	public Image Background = null;
	public Image HealthBackground = null;
	public Image Health = null;
	private Character character = null;
    // Start is called before the first frame update
    void Start()
    {
		this.character = this.GetComponentInParent<Character>();
	}

    // Update is called once per frame
    void Update()
    {
		if(character != null && this.Health != null)
		{
			this.Health.fillAmount = ((float)this.character.Health) / ((float)this.character.MaxHealth);
		}
    }
}

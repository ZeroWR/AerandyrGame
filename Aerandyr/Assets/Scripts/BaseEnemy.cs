using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, ICanTakeDamage
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void TakeDamage(GameObject sender, int damage, Vector2 force)
	{
		Debug.Log(string.Format("{0} took {1} damage from {2}", this.gameObject.name, damage, sender.name));
		var rbody = this.GetComponent<Rigidbody2D>();
		if (!rbody)
			return;
		rbody.AddForce(force);
	}
}

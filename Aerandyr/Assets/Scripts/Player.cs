using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
	private IsoCharacterController controller = null;
	private float nextTakeDamageTime = 0.0f;
    // Start is called before the first frame update
    protected override void Start()
    {
		base.Start();
		controller = GetComponent<IsoCharacterController>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public override void TakeDamage(GameObject sender, int damage, Vector2 force)
	{
		if (nextTakeDamageTime > Time.time)
			return;
		var rbody = this.GetComponent<Rigidbody2D>();
		if (!rbody)
			return;
		rbody.AddForce(force/*, ForceMode2D.Impulse*/);
		base.TakeDamage(sender, damage, force);
		if (controller)
			controller.TakeDamage(sender, damage, force);
		nextTakeDamageTime = Time.time + 0.25f;
	}
	
}

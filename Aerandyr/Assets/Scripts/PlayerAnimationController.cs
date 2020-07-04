using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : CharacterAnimationController
{
	public bool IsWalking
	{
		get { return animator != null ? animator.GetBool("MoveFlg") : false; }
		set
		{
			if (value == IsWalking)
				return;
			ChangeParameter("MoveFlg", value);
		}
	}
	private bool isFacingForwards = true;
	public bool IsFacingForwards
	{
		get { return isFacingForwards; }
		set
		{
			if (value == IsFacingForwards)
				return;
			isFacingForwards = value;
			ChangeAnimation(isFacingForwards ? "CA_Idle_1" : "CA_Idle_2");
		}
	}
	public void Attack()
	{
		ChangeAnimation(isFacingForwards ? "CA_Attack_1" : "CA_Attack_2");
	}
	// Start is called before the first frame update
	override protected void Start()
    {
		base.Start();
		ChangeAnimation("CA_Idle_1");
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}

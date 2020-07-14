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
			var animationName = GetActualAnimationName("Idle");
			ChangeAnimation(animationName);
		}
	}
	public void PlayHurtAnimation()
	{
		var animationName = GetActualAnimationName("Damage");
		ChangeAnimation(animationName);
	}
	public void Attack()
	{
		var animationName = GetActualAnimationName("Attack");
		ChangeAnimation(animationName);
	}
	public void PlayWinAnimation()
	{
		var animationName = GetActualAnimationName("Win");
		ChangeAnimation(animationName);
	}
	private string GetActualAnimationName(string animationName)
	{
		return string.Format("CA_{0}_{1}", animationName, isFacingForwards ? 1 : 2);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAnimations
{
	Idle,
	Attack1,
	Attack2,
	Walk
}

public class AnimationArgs : EventArgs
{
	public CharacterAnimations Animation { get; set; }
}

public enum CharacterAnimationEvents
{
	DoDamage
}

public class AnimationEventArgs : EventArgs
{
	public CharacterAnimationEvents AnimationEvent { get; set; }
}

public class CharacterAnimationController : MonoBehaviour
{
	private Animator animator;
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
	void Start()
    {
		animator = GetComponent<Animator>();
		if(animator == null)
			animator = GetComponentInChildren<Animator>();
		ChangeAnimation("CA_Idle_1");
	}
	// Update is called once per frame
	void Update()
    {
        
    }
	private void ChangeAnimation(string animationName)
	{
		if (animator == null)
			return;
		animator.Play(animationName);
	}
	private void ChangeParameter(string name, bool flag)
	{
		if (animator == null)
			return;
		animator.SetBool(name, flag);
	}
	public event EventHandler<AnimationEventArgs> AnimationEvent;
	private void OnAnimationEvent(CharacterAnimationEvents animationEvent)
	{
		if (AnimationEvent != null)
			AnimationEvent.Invoke(this, new AnimationEventArgs() { AnimationEvent = animationEvent });
	}
	public event EventHandler<AnimationArgs> AnimationDone;
	private void OnAnimationDone(CharacterAnimations animation)
	{
		if (AnimationDone != null)
			AnimationDone.Invoke(this, new AnimationArgs() { Animation = animation });
	}
}

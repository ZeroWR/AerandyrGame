using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAnimations
{
	Idle,
	Attack1,
	Attack2,
	Walk,
	Win
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
	protected Animator animator;
	// Start is called before the first frame update
	protected virtual void Start()
    {
		animator = GetComponent<Animator>();
		if(animator == null)
			animator = GetComponentInChildren<Animator>();
	}
	// Update is called once per frame
	void Update()
    {
        
    }
	public void ChangeAnimation(string animationName)
	{
		if (animator == null)
			return;
		animator.Play(animationName);
	}
	public void ChangeParameter(string name, bool flag)
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

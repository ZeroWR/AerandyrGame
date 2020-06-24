using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCharacterController : MonoBehaviour
{
	public float movementSpeed = 1f;
	private Rigidbody2D rbody;
	private CharacterAnimationController animationController;
	private void Awake()
	{
		rbody = GetComponent<Rigidbody2D>();
		animationController = GetComponent<CharacterAnimationController>();
	}

	private Vector2 lastCharacterMovement;
	// Update is called once per frame
	void FixedUpdate()
	{
		Vector2 currentPos = rbody.position;
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
		inputVector = Vector2.ClampMagnitude(inputVector, 1);
		Vector2 movement = inputVector * movementSpeed;
		Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
		//if(lastCharacterMovement != movement)
		//{
		//	Debug.Log(string.Format("Character movement: {0}", movement.ToString()));
		//	lastCharacterMovement = movement;
		//}
		rbody.MovePosition(newPos);
		UpdateFacingDirection(inputVector);
		UpdateAnimation(movement);
	}

	private void UpdateHorizontalDirection(Vector2 inputVector)
	{
		if (inputVector.x == 0.0f)
			return;
		Vector3 tmp = this.transform.localScale;
		var shouldBeFacingLeft = inputVector.x < 0;
		var isFacingLeft = tmp.x > 0; //Positive means we're not flipped, and we're facing left normally.
		if (shouldBeFacingLeft != isFacingLeft)
		{
			tmp.x = shouldBeFacingLeft ? Mathf.Abs(tmp.x) : -Mathf.Abs(tmp.x);
			this.transform.localScale = tmp;
		}
	}

	private void UpdateVerticalDirection(Vector2 inputVector)
	{
		if (animationController == null || inputVector.y == 0.0f)
			return;

		animationController.IsFacingForwards = inputVector.y < 0.0f;
	}

	private void UpdateFacingDirection(Vector2 inputVector)
	{
		UpdateHorizontalDirection(inputVector);
		UpdateVerticalDirection(inputVector);
	}

	private void UpdateAnimation(Vector2 movement)
	{
		if (animationController == null)
			return;
		var shouldBeWalking = movement.x != 0.0f || movement.y != 0.0f;
		if(shouldBeWalking != animationController.IsWalking)
		{
			//Debug.Log(string.Format("Character.UpdateAnimation: {0}", movement.ToString()));
			animationController.IsWalking = shouldBeWalking;
		}
	}
}

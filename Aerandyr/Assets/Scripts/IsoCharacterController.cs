using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IsoCharacterController : MonoBehaviour
{
	public float movementSpeed = 1f;
	private Rigidbody2D rbody;
	private PlayerAnimationController animationController;
	private List<IInteractable> touchingInteractables = new List<IInteractable>();
	private float nextUseTime = 0.0f;
	private float nextAttackTime = 0.0f;
	public GameObject SwordDamageTrigger = null;
	private Player playerCharacter = null;
	public Player PlayerCharacter { get { return playerCharacter; } }
	private HUD hud = null;
	public HUD HUD { get { return hud; } }
	private bool isInCutscene = false;
	private bool canMove = true;
	private float nextCanMoveTime = 0.0f;
	private List<Quest> ourQuests = new List<Quest>();
	private Quest currentQuest = null;
	public Quest CurrentQuest { get { return this.currentQuest; } }
	private void Awake()
	{
		rbody = GetComponent<Rigidbody2D>();
		animationController = GetComponent<PlayerAnimationController>();
		if(animationController != null)
		{
			animationController.AnimationEvent += AnimationController_AnimationEvent;
		}
		this.playerCharacter = GetComponent<Player>();
		this.hud = GetComponent<HUD>();
		if (this.hud && this.playerCharacter)
			this.hud.Controller = this;
	}

	private void Update()
	{
		if(this.IsInDialog)
		{
			//Forward keypress here, or do in HUD?
			this.HUD.ProcessInput();
		}
		else if(!isInCutscene)
			ProcessInput();

		if (!this.canMove && this.nextCanMoveTime <= Time.time)
		{
			this.canMove = true;
		}
	}
	private void ProcessInput()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			DoUse();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			DoAttack();
		}
	}

	public void TakeDamage(GameObject sender, int damage, Vector2 force)
	{
		this.nextCanMoveTime = Time.time + 0.25f;
		this.canMove = false;
		if(this.animationController != null)
		{
			this.animationController.IsWalking = false;
			this.animationController.PlayHurtAnimation();
		}
	}

	#region Dialog/HUD
	private bool IsInDialog { get { return this.HUD != null && this.HUD.IsInDialog; } }
	public Task<bool> InventoryAcquiredNotification(Dialog dialog)
	{
		if (this.isInCutscene)
			return Task.FromResult(false);
		var tcs = new TaskCompletionSource<bool>();
		this.isInCutscene = true;
		this.animationController.IsFacingForwards = true;
		EventHandler<AnimationArgs> animationDoneCallback = null;
		animationDoneCallback = (object sender, AnimationArgs e) =>
		{
			if (e.Animation != CharacterAnimations.Win)
				return;
			this.HUD.ShowDialog(dialog);
			this.animationController.AnimationDone -= animationDoneCallback;
			this.isInCutscene = false;
			tcs.SetResult(true);
		};
		this.animationController.AnimationDone += animationDoneCallback;
		this.animationController.PlayWinAnimation();
		return tcs.Task;
	}
	public void ReceivedQuest(Quest quest)
	{
		if (ourQuests.Contains(quest))
			return;
		ourQuests.Add(quest);
		if (this.currentQuest == null)
			this.currentQuest = quest;

		this.HUD.ReceivedQuest(quest);
	}
	public bool HasQuest(Quest quest)
	{
		return this.ourQuests.Contains(quest);
	}
	#endregion

	#region Attack
	private bool CanAttack { get { return nextAttackTime <= Time.time; } }
	private void DoAttack()
	{
		if (!CanAttack || !animationController)
			return;
		animationController.Attack();
		nextAttackTime = Time.time + 0.5f;
	}
	#endregion

	#region Movement
	// Update is called once per frame
	void FixedUpdate()
	{
		if (this.IsInDialog || !this.canMove)
			return;
		Vector2 currentPos = rbody.position;
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
		inputVector = Vector2.ClampMagnitude(inputVector, 1);
		Vector2 movement = inputVector * movementSpeed;
		Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
		rbody.velocity = movement * Time.fixedDeltaTime;
		//rbody.MovePosition(newPos);
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
		if (shouldBeWalking != animationController.IsWalking)
			animationController.IsWalking = shouldBeWalking;
	}

	#endregion

	#region Use
	private bool CanDoUse
	{
		get { return nextUseTime <= Time.time && touchingInteractables.Count > 0; }
	}
	private void DoUse()
	{
		if (!CanDoUse)
			return;

		foreach (var interactable in touchingInteractables)
		{
			if (interactable.CanInteract(this))
				interactable.Interact(this);
		}
		nextUseTime = Time.time + 0.25f;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		var interactable = collision.gameObject.GetComponent<IInteractable>();
		if (interactable == null || touchingInteractables.Contains(interactable))
			return;
		touchingInteractables.Add(interactable);
	}

	//Hitting a collider 2D
	private void OnCollisionStay2D(Collision2D collision)
	{
		//Do something
	}

	//Just stop hitting a collider 2D
	private void OnCollisionExit2D(Collision2D collision)
	{
		var interactable = collision.gameObject.GetComponent<IInteractable>();
		if (interactable == null || !touchingInteractables.Contains(interactable))
			return;
		touchingInteractables.Remove(interactable);
		//Do something
	}
	#endregion

	#region Callbacks
	private void AnimationController_AnimationEvent(object sender, AnimationEventArgs e)
	{
		if (e.AnimationEvent == CharacterAnimationEvents.DoDamage && SwordDamageTrigger != null)
		{
			var doDamageTrigger = SwordDamageTrigger.GetComponent<DoDamageTrigger>();
			if (!doDamageTrigger)
				return;
			doDamageTrigger.DoDamageToAll(this.gameObject, 10, this.transform.position);
		}
	}
	#endregion
}

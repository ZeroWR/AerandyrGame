using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class InventoryItem
{
	public ItemDefinition ItemDefintion { get; set; }
	public int Quantity { get; set; }
}

public class IsoCharacterController : MonoBehaviour
{
	#region Properties And Members
	private static int controllerCount = 0;
	private int controllerId = 0;
	public int ControllerId { get { return controllerId; } }

	//There are too many members in this class.  Need to break some out.
	public float movementSpeed = 1f;
	private Rigidbody2D rbody;
	private PlayerAnimationController animationController;
	private List<IInteractable> touchingInteractables = new List<IInteractable>();
	private float nextUseTime = 0.0f;
	private float nextAttackTime = 0.0f;
	public GameObject SwordDamageTrigger = null;
	private Player playerCharacter = null;
	public Player PlayerCharacter { get { return playerCharacter; } }
	public HUD HUD { get; protected set; }
	private bool isInCutscene = false;
	private bool canMove = true;
	private float nextCanMoveTime = 0.0f;
	private List<Quest> ourQuests = new List<Quest>();
	public ReadOnlyCollection<Quest> Quests { get { return ourQuests.AsReadOnly(); } }
	private Quest currentQuest = null;
	public Quest CurrentQuest { get { return this.currentQuest; } }
	private PauseMenu pauseMenu;
	private float nextUIKeyDownTime = 0.0f;

	public HUD HUDPrefab;
	public PauseMenu PauseMenuPrefab;

	private List<InventoryItem> inventory = new List<InventoryItem>();
	public ReadOnlyCollection<InventoryItem> Inventory { get { return inventory.AsReadOnly(); } }
	#endregion

	private void Awake()
	{
		controllerCount++;
		this.controllerId = controllerCount;
		rbody = GetComponent<Rigidbody2D>();
		animationController = GetComponent<PlayerAnimationController>();
		if(animationController != null)
		{
			animationController.AnimationEvent += AnimationController_AnimationEvent;
		}
		this.playerCharacter = GetComponent<Player>();
		if(this.HUDPrefab != null)
		{
			this.HUD = Instantiate<HUD>(HUDPrefab);
			this.HUD.transform.parent = this.gameObject.transform;
			this.HUD.Controller = this;
		}
		if(this.HUD == null)
		{
			this.HUD = GetComponentInChildren<HUD>();
			if (this.HUD)
				this.HUD.Controller = this;
		}
		if (this.PauseMenuPrefab != null)
		{
			this.pauseMenu = Instantiate(PauseMenuPrefab);
			this.pauseMenu.transform.parent = this.gameObject.transform;
			this.pauseMenu.Controller = this;
			this.pauseMenu.Hide();
		}
	}

	private void Update()
	{
		if(this.IsInDialog)
		{
			//Forward keypress here, or do in HUD?
			this.HUD.ProcessInput();
		}
		else if (!isInCutscene)
			ProcessInput();

		if (this.IsInUI)
			this.pauseMenu.ProcessInput();

		if (!this.canMove && this.nextCanMoveTime <= Time.time)
		{
			this.canMove = true;
		}
	}
	private void ProcessInput()
	{
		if(!this.IsInUI)
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
		if(Input.GetButtonDown("Pause"))
		{
			TogglePauseMenu();
		}
		if (Input.GetButtonDown("Inventory"))
		{
			ToggleInventory();
		}
	}

	#region Pickups/Inventory
	public bool CanPickUpItem(ItemDefinition itemDefinition, int quantity)
	{
		var existingInventoryItem = this.inventory.Find(x => x.ItemDefintion.ID == itemDefinition.ID);
		if (existingInventoryItem == null)
			return true;

		return itemDefinition.MaxCarry < 0 ? true : existingInventoryItem.Quantity + quantity <= itemDefinition.MaxCarry;
	}
	public void PickupItem(ItemDefinition itemDefinition, int quantity)
	{
		var existingInventoryItem = this.inventory.FirstOrDefault(x => x.ItemDefintion.ID == itemDefinition.ID);
		if (existingInventoryItem == null)
		{
			existingInventoryItem = new InventoryItem()
			{
				ItemDefintion = itemDefinition
			};
			inventory.Add(existingInventoryItem);
		}
		existingInventoryItem.Quantity += quantity;

		//For testing inventory screen:
		//for (int i = 1; i <= 20; ++i)
		//{
		//	var existingInventoryItem = new InventoryItem()
		//	{
		//		ItemDefintion = itemDefinition,
		//		Quantity = i
		//	};
		//	inventory.Add(existingInventoryItem);
		//}
	}
	#endregion

	#region TakeDamage
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
	#endregion

	#region Dialog/HUD/UI
	public void ShowDialog(Dialog dialog)
	{
		this.rbody.velocity = Vector2.zero;
		this.HUD.ShowDialog(dialog);
	}
	private bool IsInDialog { get { return this.HUD != null && this.HUD.IsInDialog; } }
	private bool IsInUI { get { return this.pauseMenu != null && this.pauseMenu.enabled; } }
	public Task<bool> InventoryAcquiredNotification(Dialog dialog)
	{
		if (this.isInCutscene)
			return Task.FromResult(false);
			
		var tcs = new TaskCompletionSource<bool>();
		this.isInCutscene = true;
		this.animationController.IsFacingForwards = true;
		this.rbody.velocity = Vector2.zero;
		EventHandler<AnimationArgs> animationDoneCallback = null;
		animationDoneCallback = (object sender, AnimationArgs e) =>
		{
			if (e.Animation != CharacterAnimations.Win)
				return;
			this.ShowDialog(dialog);
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
	private bool CanTogglePauseMenu
	{
		get { return nextUIKeyDownTime <= Time.time; }
	}
	private void TogglePauseMenu()
	{
		if (!this.pauseMenu || !CanTogglePauseMenu)
			return;

		this.pauseMenu.Toggle();
		this.HUD.gameObject.SetActive(!this.pauseMenu.enabled);
		nextUIKeyDownTime = Time.time + 0.25f;
	}
	private void ToggleInventory()
	{
		if (!this.pauseMenu || !CanTogglePauseMenu)
			return;

		this.pauseMenu.Toggle("Inventory");
		this.HUD.gameObject.SetActive(!this.pauseMenu.enabled);
		nextUIKeyDownTime = Time.time + 0.25f;
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
		if (this.IsInDialog || !this.canMove || this.IsInUI || this.isInCutscene)
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public float maxMoveForce = 1;
	public float moveScale = 50;
	public float bulletScale = 200;
	public float maxVelocity = 10;
	private Rigidbody2D rigidBody;
	private Collider2D collider2D;
	private SpriteRenderer spriteRenderer;

	private HashSet<Item> itemsUnderfoot;

	private Animator anim;
	public Weapon[] heldWeapons;
	public int weaponToReplace = 0;

	public int maxHealth = 20;
	public int currHealth = 10;
	[HideInInspector]
	public Hands hands;

	private Vector2[] WEAPON_OFFSET_DOWN = {new Vector2(-0.28f, -0.22f), new Vector2(0.28f, -0.22f)};
	private Vector2[] WEAPON_OFFSET_UP = {new Vector2(-0.29f, -0.12f), new Vector2(0.29f, -0.12f)};
	private Vector2[] WEAPON_OFFSET_LEFT = {new Vector2(-0.21f, -0.21f), new Vector2(-0.26f, -0.13f)};
	private Vector2[] WEAPON_OFFSET_RIGHT = {new Vector2(0.27f, -0.12f), new Vector2(0.23f, -0.2f)};


	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		collider2D = this.GetComponent<Collider2D>();
		itemsUnderfoot = new HashSet<Item>();
		heldWeapons = new Weapon[2];
		hands = GetComponentInChildren<Hands>();
		anim = GetComponent<Animator>();
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
	}

	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		UpdateSprite(mousePos);
		

		//INPUT LAYER

		//Move Player mapped to joysticks
		MovePlayer();
		//Left / Right Mouse Clicks
		if (Input.GetMouseButtonDown(0)){
			FireWeapon(hands.left.weapon, mousePos);
		}

		if (Input.GetMouseButtonDown(1)){
			FireWeapon(hands.right.weapon, mousePos);
		}
		//Other Functionality
		if(Input.GetKeyDown(KeyCode.Q)) PickupItems(hands.left);
		if(Input.GetKeyDown(KeyCode.E)) PickupItems(hands.right);	
	}

	private void FireWeapon(Weapon weapon, Vector3 mousePos){
		if (!weapon)
			return;
		weapon.FireBullet(mousePos);
	}

	private void UpdateSprite(Vector3 mousePos){
		//AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		Vector3 relativePos = mousePos - transform.position;
		float angle = Vector2.Angle(Vector2.right, relativePos);
		if (relativePos.y < 0) {
			angle = 360 - angle;
		}
		int dir;

		//Check for movement
		if (rigidBody.velocity == Vector2.zero) {
			anim.SetBool("Movement", false);
		} else {
			anim.SetBool("Movement", true);
		}

		//Find Direction
		if (angle > 45 && angle <= 135) {
			dir = 0;
		} else if (angle > 135 && angle <= 225) {
			dir = 3;
		} else if (angle > 225 && angle <= 315) {
			dir = 2;
		} else {
			dir = 1;
		}

		//Update Sprites
		anim.SetFloat("direction", dir);
	}

	private void MovePlayer(){
		Vector2 sumForces = Input.GetAxis("Vertical") * Vector2.up + Input.GetAxis("Horizontal") * Vector2.right;
		sumForces.Normalize();

		rigidBody.AddForce(sumForces * moveScale, ForceMode2D.Impulse);
		if (rigidBody.velocity.sqrMagnitude > maxVelocity*maxVelocity) {
			float diff = rigidBody.velocity.magnitude - maxVelocity;
			rigidBody.AddForce(rigidBody.velocity.normalized * diff);
		}
		//Debug.Log(transform.position);
	}

	void OnTriggerEnter2D(Collider2D other){
		Item itemUnder = other.gameObject.GetComponent<Item>();
//		Debug.Log(itemUnder);
		itemsUnderfoot.Add(itemUnder);
	}

	void OnTriggerExit2D(Collider2D other){
		itemsUnderfoot.Remove(other.gameObject.GetComponent<Item>());
	}

	private void PickupItems(Hand hand){
		Item result = null;
		//Attempt to pick up item from pool. Breaks upon success
		foreach (Item item in itemsUnderfoot) {
			bool picked = item.Pickup(hand); //Item will handle the logic to swap item
			if (picked) {
				result = item;
				break;
			} 
		}
		//Remove picked-up item from pool
		if (result) {
			itemsUnderfoot.Remove(result);
		}

	}
}

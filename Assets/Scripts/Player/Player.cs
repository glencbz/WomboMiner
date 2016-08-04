using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Creature {
	public float invul_frame = 0.5f;
	private bool invul_flag = false;
	private float invul_timer;

	//Private Entities

	private Animator anim;
	private HashSet<Item> itemsUnderfoot;

	[HideInInspector]
	public Hands hands;


	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		itemsUnderfoot = new HashSet<Item>();
		hands = GetComponentInChildren<Hands>();
		anim = GetComponent<Animator>();
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
	}

	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		UpdateSprite(mousePos);

		//TIME LAYER

		//Invulnerability frame time cooldown.
		if (invul_flag) {
			invul_timer -= Time.deltaTime;
			if (invul_timer <= 0) {
				invul_flag = false;
			}
		}

		

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
		if (other.tag == "Item") {
			Item itemUnder = other.gameObject.GetComponent<Item>();
			itemsUnderfoot.Add(itemUnder);
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Item") {
			itemsUnderfoot.Remove(other.gameObject.GetComponent<Item>());
		}
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

	public override void takeDamage(int dmg) {
		//Take damage only if not in invul frame
		if (!invul_flag) {
			currHealth -= dmg;
			GetComponent<takeDamageFlash>().flash(invul_frame);
			//Damage triggers invul_frame
			invul_flag = true;
			invul_timer = invul_frame;
		}

	}
}

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
	public GameObject leftHand, rightHand;

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
		anim = GetComponent<Animator>();
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		// leftHand = this.transform.GetChild(1).gameObject;
		// rightHand = this.transform.GetChild(2).gameObject;
	}

	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		UpdateSprite(mousePos);
		MovePlayer();
		if (Input.GetMouseButtonDown(0)){
			FireWeapon(heldWeapons[0], mousePos);
		}

		if (Input.GetMouseButtonDown(1)){
			FireWeapon(heldWeapons[1], mousePos);
		}

		if(Input.GetKeyDown(KeyCode.E)) PickupItems(1);
		if(Input.GetKeyDown(KeyCode.Q)) PickupItems(0);	
	}

	private void FireWeapon(Weapon weapon, Vector3 mousePos){
		if (!weapon)
			return;
		weapon.FireBullet(mousePos);
	}

	private void UpdateSprite(Vector3 mousePos){
		//AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		float angle = Vector2.Angle(Vector2.up, transform.position - mousePos);
		int dir;

		//Check for movement
		if (rigidBody.velocity == Vector2.zero) {
			anim.SetBool("Movement", false);
		} else {
			anim.SetBool("Movement", true);
		}

		//Find Direction
		if (angle < 45) {								//Face Down
			dir = 2;
		} else if (angle > 135) {						//Face Up
			dir = 0;
		} else if (mousePos.x < transform.position.x) {	//Face Left
			dir = 3;
		} else {										//Face Right
			dir = 1;
		}

		anim.SetFloat("direction", dir);
		updateWeaponSprite(dir, mousePos);
	}

	private void updateWeaponSprite(int dir, Vector2 mousePos) {
		// switch(dir) {
		// 	case 0:
		// 		leftHand.transform.localPosition = WEAPON_OFFSET_UP[0];
		// 		rightHand.transform.localPosition = WEAPON_OFFSET_UP[1];
		// 		leftHand.GetComponent<SpriteRenderer>().flipX = false;
		// 		rightHand.GetComponent<SpriteRenderer>().flipX = false;
		// 		leftHand.GetComponent<SpriteRenderer>().flipY = true;
		// 		rightHand.GetComponent<SpriteRenderer>().flipY = true;
		// 		break;
		// 	case 1:
		// 		leftHand.transform.localPosition = WEAPON_OFFSET_RIGHT[0];
		// 		rightHand.transform.localPosition = WEAPON_OFFSET_RIGHT[1];
		// 		leftHand.GetComponent<SpriteRenderer>().flipX = false;
		// 		rightHand.GetComponent<SpriteRenderer>().flipX = false;
		// 		leftHand.GetComponent<SpriteRenderer>().flipY = false;
		// 		rightHand.GetComponent<SpriteRenderer>().flipY = false;
		// 		break;	
		// 	case 2:
		// 		leftHand.transform.localPosition = WEAPON_OFFSET_DOWN[0];
		// 		rightHand.transform.localPosition = WEAPON_OFFSET_DOWN[1];
		// 		leftHand.GetComponent<SpriteRenderer>().flipX = false;
		// 		rightHand.GetComponent<SpriteRenderer>().flipX = true;
		// 		leftHand.GetComponent<SpriteRenderer>().flipY = false;
		// 		rightHand.GetComponent<SpriteRenderer>().flipY = false;
		// 		break;
		// 	case 3:
		// 		leftHand.transform.localPosition = WEAPON_OFFSET_LEFT[0];
		// 		rightHand.transform.localPosition = WEAPON_OFFSET_LEFT[1];
		// 		leftHand.GetComponent<SpriteRenderer>().flipX = true;
		// 		rightHand.GetComponent<SpriteRenderer>().flipX = true;
		// 		leftHand.GetComponent<SpriteRenderer>().flipY = false;
		// 		rightHand.GetComponent<SpriteRenderer>().flipY = false;
		// 		break;
		// }

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

	private void PickupItems(int hand){
		foreach (Item item in itemsUnderfoot)
			item.Pickup(hand);
	}
}

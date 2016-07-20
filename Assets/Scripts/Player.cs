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

	int SpriteUpHash = Animator.StringToHash("up");
	int SpriteDownHash = Animator.StringToHash("down");
	int SpriteLeftHash = Animator.StringToHash("left");
	int SpriteRightHash = Animator.StringToHash("right");

	int SpriteStateUp = Animator.StringToHash("player_move_back");
	int SpriteStateDown = Animator.StringToHash("Base.player_move_front");
	int SpriteStateLeft = Animator.StringToHash("player_move_left");
	int SpriteStateRight = Animator.StringToHash("player_move_right");
	public Weapon[] heldWeapons;
	public int weaponToReplace = 0;

	public float maxHealth;
	public float currHealth;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		collider2D = this.GetComponent<Collider2D>();
		itemsUnderfoot = new HashSet<Item>();
		heldWeapons = new Weapon[2];
		anim = GetComponent<Animator>();
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		Debug.Log(stateInfo.nameHash);
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

		if(Input.GetKeyDown(KeyCode.Space))
			PickupItems();
	}

	private void FireWeapon(Weapon weapon, Vector3 mousePos){
		if (!weapon)
			return;
		weapon.FireBullet(mousePos);
	}

	private void UpdateSprite(Vector3 mousePos){
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		float angle = Vector2.Angle(Vector2.up, transform.position - mousePos);

		if (angle < 45) {
			//Face Down
			if (stateInfo.fullPathHash != SpriteStateDown) {anim.SetTrigger (SpriteDownHash);}
			
		} else if (angle > 135) {
			//Face Up
			if (stateInfo.fullPathHash != SpriteStateUp) {anim.SetTrigger (SpriteUpHash); };
		} else if (mousePos.x < transform.position.x) {
			//Face Left
			anim.SetTrigger(SpriteLeftHash);
		} else {
			//Face Right
			anim.SetTrigger(SpriteRightHash);
		}

		// if (rigidBody.velocity == Vector2.zero) {
		// 	anim.speed = 0;
		// } else {
		// 	anim.speed = 1;
		// }
		// if (mousePos.x < transform.position.x)
		// 	spriteRenderer.flipX = true;
		// else
		// 	spriteRenderer.flipX = false;
	}

	private void MovePlayer(){
		Vector2 sumForces = Vector2.zero;

		sumForces = Input.GetAxis("Vertical") * Vector2.up + Input.GetAxis("Horizontal") * Vector2.right;
		sumForces.Normalize();

		rigidBody.AddForce(sumForces * moveScale, ForceMode2D.Impulse);
		if (rigidBody.velocity.sqrMagnitude > maxVelocity*maxVelocity) {
			float diff = rigidBody.velocity.magnitude - maxVelocity;
			rigidBody.AddForce(rigidBody.velocity.normalized * diff);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		Item itemUnder = other.gameObject.GetComponent<Item>();
//		Debug.Log(itemUnder);
		itemsUnderfoot.Add(itemUnder);
	}

	void OnTriggerExit2D(Collider2D other){
		itemsUnderfoot.Remove(other.gameObject.GetComponent<Item>());
	}

	private void PickupItems(){
		foreach (Item item in itemsUnderfoot)
			item.Pickup();
	}
}

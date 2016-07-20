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

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		collider2D = this.GetComponent<Collider2D>();
		itemsUnderfoot = new HashSet<Item>();
		heldWeapons = new Weapon[2];
		anim = GetComponent<Animator>();
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
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
		//AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		float angle = Vector2.Angle(Vector2.up, transform.position - mousePos);

		//Update Sprite according to facing direction
		if (angle < 45) {
			//Face Down
			anim.SetFloat("direction", 2);
			//if (stateInfo.fullPathHash != SpriteStateDown) {anim.SetTrigger (SpriteDownHash); }
		} else if (angle > 135) {
			//Face Up
			anim.SetFloat("direction", 0);
			//if (stateInfo.fullPathHash != SpriteStateUp) {anim.SetTrigger (SpriteUpHash); }
		} else if (mousePos.x < transform.position.x) {
			//Face Left
			anim.SetFloat("direction", 3);
			//if (stateInfo.fullPathHash != SpriteStateLeft) {anim.SetTrigger(SpriteLeftHash); }
		} else {
			//Face Right
			anim.SetFloat("direction", 1);
			//if (stateInfo.fullPathHash != SpriteStateRight) {anim.SetTrigger(SpriteRightHash); }
		}

		//If Player is static, stop animation
		if (rigidBody.velocity == Vector2.zero) {
			anim.SetBool("Movement", false);
		} else {
			anim.SetBool("Movement", true);
		}
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

	private void PickupItems(){
		foreach (Item item in itemsUnderfoot)
			item.Pickup();
	}
}

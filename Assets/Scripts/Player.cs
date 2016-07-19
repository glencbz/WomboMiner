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
	}

	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		FlipPlayer(mousePos);
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

	private void FlipPlayer(Vector3 mousePos){
		if (mousePos.x < transform.position.x)
			spriteRenderer.flipX = true;
		else
			spriteRenderer.flipX = false;
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

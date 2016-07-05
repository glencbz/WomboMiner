using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public float maxMoveForce = 1;
	public float moveScale = 50;
	public float bulletScale = 200;
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
	
	}

	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		FlipPlayer(mousePos);

		if (Input.GetMouseButtonDown(0)){
			FireWeapon(heldWeapons[0], mousePos);
		}

		if (Input.GetMouseButtonDown(1)){
			FireWeapon(heldWeapons[1], mousePos);
		}

		MovePlayer();
	}

	private void FireWeapon(Weapon weapon, Vector3 mousePos){
		if (!weapon)
			return;

		Vector2 fireDirection = (Vector2) (mousePos - transform.position);
		weapon.FireBullet(fireDirection);
	}

	private void FlipPlayer(Vector3 mousePos){
		if (mousePos.x < transform.position.x)
			spriteRenderer.flipX = true;
		else
			spriteRenderer.flipX = false;
	}

	private void MovePlayer(){
		Vector2 sumForces = Vector2.zero;

		if ( Input.GetKey(KeyCode.UpArrow) )
			sumForces += Vector2.up;
		if ( Input.GetKey(KeyCode.DownArrow) )
			sumForces += Vector2.down;
		if ( Input.GetKey(KeyCode.RightArrow) )
			sumForces += Vector2.right;
		if ( Input.GetKey(KeyCode.LeftArrow) )
			sumForces += Vector2.left;

		sumForces.Normalize();
		rigidBody.AddForce(sumForces * moveScale);
	}

	void OnTriggerEnter2D(Collider2D other){
		Item itemUnder = other.gameObject.GetComponent<Item>();
		itemsUnderfoot.Add(itemUnder);
	}

	void OnTriggerExit2D(Collider2D other){
		itemsUnderfoot.Remove(other.gameObject.GetComponent<Item>());
	}

	private void PickupWeapon(){
		foreach (Item item in itemsUnderfoot)
			item.Pickup();
	}
}

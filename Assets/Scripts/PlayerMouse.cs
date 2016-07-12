using UnityEngine;
using System.Collections;

public class PlayerMouse : MonoBehaviour {

	public GameObject weapon;
	public float moveScale = 50;
	public float bulletScale = 200;
	private Rigidbody2D rigidBody;
	private Collider2D collider2D;
	private GameObject weaponUnderfoot;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		collider2D = GetComponent<Collider2D>();
	}

	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//RotatePlayer(mousePos);

		if (Input.GetMouseButtonDown(0)){
			FireBullet(mousePos);
		}

		if (Input.GetMouseButtonDown(1)){
			PickupWeapon();
		}

		MovePlayer();
	}

	// these only work on the assumption you can only collide with weapons
	void OnTriggerEnter2D(Collider2D other){
		weaponUnderfoot = other.gameObject;
	}

	void OnTriggerExit2D(Collider2D other){
		weaponUnderfoot = null;
	}

	private void RotatePlayer(Vector3 mousePos){
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position) ;
		rotation *= Quaternion.Euler(0, 0, 90);

		transform.rotation = rotation;
	}

	private void FireBullet(Vector3 mousePos){
		// extremely crude cloning of whatever thing is on the floor

		//TODO: create bullets that are hidden by default and then clone those instead
		if (!weapon)
			return;
		
		GameObject newThing = Instantiate(weapon, transform.position, Quaternion.identity) as GameObject;
		Collider2D newThingCollider = newThing.GetComponent<Collider2D>();
		Physics2D.IgnoreCollision(collider2D, newThingCollider);
	
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			Physics2D.IgnoreCollision(newThingCollider, enemy.GetComponent<Collider2D>());
			
		Vector2 forceDirection = (Vector2) (mousePos - transform.position);
		newThing.GetComponent<Rigidbody2D>().AddForce(forceDirection * bulletScale);
	}

	private void PickupWeapon(){
		if (weaponUnderfoot){
			weapon = weaponUnderfoot;
		}		
	}

	private void MovePlayer(){
		if ( Input.GetKey(KeyCode.UpArrow) )
			rigidBody.AddForce(Vector2.up * moveScale);
		if ( Input.GetKey(KeyCode.DownArrow) )
			rigidBody.AddForce(Vector2.down * moveScale);
		if ( Input.GetKey(KeyCode.RightArrow) )
			rigidBody.AddForce(Vector2.right * moveScale);
		if ( Input.GetKey(KeyCode.LeftArrow) )
			rigidBody.AddForce(Vector2.left * moveScale);
	}
}

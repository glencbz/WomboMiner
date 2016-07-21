using UnityEngine;
using System.Collections;

abstract public class Item : MonoBehaviour {

	protected Rigidbody2D rigidBody;
	protected Collider2D collider2D;

	virtual protected void Start () {
		rigidBody = GetComponent<Rigidbody2D>();

		collider2D = GetComponent<Collider2D>();
		collider2D.isTrigger = true;
		Debug.Log("STARTED ITEM");
	}
		
	//Pickup method called by Player when attempting to pickup. Returns true when pickup is successful
	abstract public bool Pickup(Hand hand);

	public void ToggleCollider(bool b) {
		collider2D.enabled = b;
	}

}

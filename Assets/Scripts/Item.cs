using UnityEngine;
using System.Collections;

abstract public class Item : MonoBehaviour {

	protected Rigidbody2D rigidBody;
	protected Collider2D collider2D;

	virtual protected void Start () {
		rigidBody = GetComponent<Rigidbody2D>();

		collider2D = GetComponent<Collider2D>();
		collider2D.isTrigger = true;
	}
		
	abstract public void Pickup();
}

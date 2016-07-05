using UnityEngine;
using System.Collections;

abstract public class Bullet : MonoBehaviour {

	private Rigidbody2D rigidBody;
	private Collider2D collider2D;

	abstract protected void FireBehaviour();
	abstract protected void OnTriggerEnter2D();

	virtual protected void Start () {
		rigidBody = GetComponent<Rigidbody2D>();

		collider2D = GetComponent<Collider2D>();
		collider2D.isTrigger = true;
	}
		
	void FixedUpdate(){
		FireBehaviour();
	}

	void Update () {
	
	}

	public virtual void InitialFire(Vector2 initialDirection){
		rigidBody.AddForce(initialDirection.normalized);
	}
}

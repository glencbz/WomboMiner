using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Rigidbody2D rigidBody;
	private Collider2D collider2D;
	public float speed = 100;

	void Awake () {
		rigidBody = this.GetComponent<Rigidbody2D>();
		collider2D = this.GetComponent<Collider2D>();
		collider2D.isTrigger = true;
	}
		
	void FixedUpdate(){
		FireBehaviour();
	}

	void Update () {
		if (!rigidBody)
			rigidBody = this.GetComponent<Rigidbody2D>();
		if (!collider2D){
			collider2D = this.GetComponent<Collider2D>();
			collider2D.isTrigger = true;				
		}
	}

	public virtual void InitialFire(Transform parent, Vector3 mousePos){
		transform.SetParent(parent, false);
		Debug.Log(transform.localPosition);
		Debug.Log(mousePos);
		Vector2 initialDirection = mousePos - transform.position;
		rigidBody.AddForce(initialDirection.normalized * speed);
	}

	protected virtual void FireBehaviour(){
		
	}
	protected virtual void OnTriggerEnter2D(){
		
	}
}

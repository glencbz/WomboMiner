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

	}

	public virtual void InitialFire(Transform parent, Vector3 mousePos){
		Vector2 initialDirection = mousePos - transform.position;
		rigidBody.velocity = initialDirection.normalized * speed;
	}

	//Bullet Update Method. Override for custom behaviour per frame
	protected virtual void FireBehaviour(){
		
	}
	protected virtual void OnTriggerEnter2D(){
		
	}

	//Bullet hitscan Method. Used for physical swings where we want controlled instances of damage.
	public virtual void hitScan() {

	}
}

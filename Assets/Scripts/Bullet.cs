using UnityEngine;
using System.Collections;

/*
Base class for Bullet
Requires:
	Rigidbody2D
	Collider2D

For Melee Weapon, this is the hitbox. Use hitscan for damage.
For Ranged Weapon, this is the projectile. Use OnTriggerEnter2D for damage.
	

*/
public class Bullet : MonoBehaviour {
	
	private Rigidbody2D rigidBody;
	private Collider2D collider2D;
	public float speed = 100;
	public string source;
	public int damage = 1;

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

	protected virtual void OnTriggerEnter2D(Collider2D other){
		//Environment Resolution
		switch(other.tag) {
			case "Wall":
				Destroy(gameObject);
				return;
			case "Player":
			case "Enemy":
				if (other.tag != source) {
					other.GetComponent<Creature>().takeDamage(damage);
					Destroy(gameObject);
				}
				break;
		}
	}

	//Bullet hitscan Method. Used for physical swings where we want controlled instances of damage.
	public virtual void hitScan() {

	}
}

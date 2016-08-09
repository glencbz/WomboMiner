using UnityEngine;
using System.Collections;

public class Boop : Bullet {

	public float pushback = 1000f;
	public float duration = .5f;

	void Start(){
		Invoke("DestroySelf", duration);
	}

	public override void InitialFire(Transform parent, Vector3 mousePos){
		speed = 0;
	}

	//TODO: make boop cone-shaped
	protected virtual void OnTriggerEnter2D(Collider2D other){
		//Environment Resolution
		Debug.Log(other.gameObject);
		switch(other.tag) {
			case "Player":
			case "Enemy":
				if (other.tag != source) {
					Creature otherCreature = other.GetComponent<Creature>();
					Debug.Log(otherCreature);
					Vector3 forceDirection = (otherCreature.transform.position - transform.position).normalized;
					Debug.Log(forceDirection);
					otherCreature.GetComponent<Rigidbody2D>().AddForce(forceDirection * pushback);
					otherCreature.takeDamage(damage);
	//				Destroy(gameObject);
				}
				break;
		}
	}
}

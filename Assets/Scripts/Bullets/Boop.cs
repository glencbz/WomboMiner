using UnityEngine;
using System.Collections;

// "Bullet" that pushes back enemies in front of the player (but does not travel) Creates a hitbox that destroys itself after a period of time
public class Boop : Bullet {

	public float pushback = 10000f;
	public float duration = .5f;
	public float spread = 30f;
	public float moveTime = 0.5f;
	public float boopOffset = -.2f;
	private Vector3 boopDirection;
	private Vector3 playerInitial;

	void Start(){
		Invoke("DestroySelf", duration);
		playerInitial = GameObject.FindGameObjectWithTag("Player").transform.position;
		boopDirection = transform.position - playerInitial;
	}

	public override void InitialFire(Transform parent, Vector3 mousePos){
		speed = 0;
		// sets the bullet offset to be further back from the gun so that it is not too narrow in front of the player
		transform.position += (mousePos - transform.position).normalized * boopOffset;
	}


	// returns a boolean of whether the target is within the cone of the gun (to prevent pushback in a circular area)
	private bool ShouldBoop(Vector3 otherPosition){
		Vector3 vectorToOther = otherPosition - playerInitial;
		return Vector3.Angle(vectorToOther, boopDirection) < spread;
	}

	// pushes back enemies in a cone shape in front of the gun
	protected override void OnTriggerEnter2D(Collider2D other){
		//Environment Resolution
		Debug.Log(other.gameObject);
		switch(other.tag) {
			case "Player":
			case "Enemy":
				if (other.tag != source && ShouldBoop(other.transform.position)) {
					Creature otherCreature = other.GetComponent<Creature>();
					Vector3 forceDirection = (otherCreature.transform.position - transform.position).normalized;
					otherCreature.GetComponent<Rigidbody2D>().AddForce(forceDirection * pushback, ForceMode2D.Impulse);
					otherCreature.takeDamage(damage);
				}
				break;
		}
	}
}

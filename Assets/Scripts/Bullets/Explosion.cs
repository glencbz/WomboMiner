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
public class Explosion : Bullet {

	public float duration = 0.5f;

	void Start(){
		Invoke("DestroySelf", duration);
	}

	protected override void OnTriggerEnter2D(Collider2D other){
		//Environment Resolution
		switch(other.tag) {
		case "Wall":
			other.GetComponent<Wall>().DamageWall(1);
			break;
		case "Player":
		case "Enemy":
			if (other.tag != source) 
				other.GetComponent<Creature>().takeDamage(damage);
				ApplyKnockback(other);
			break;
		}
	}
		
}

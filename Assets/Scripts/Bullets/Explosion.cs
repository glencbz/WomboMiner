using UnityEngine;
using System.Collections;

// Generic explosion-type bullet that deals damage in an area of effect and does not travel
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

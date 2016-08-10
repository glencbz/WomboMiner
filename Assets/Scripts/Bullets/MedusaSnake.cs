using UnityEngine;
using System.Collections;

public class MedusaSnake : Bullet {
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.tag);
		switch(other.tag) {
			case "Wall":
				Debug.Log("Smack wall");
				Destroy(gameObject);
				other.GetComponent<Wall>().DamageWall(1);
				
				return;
			case "Player":
			case "Enemy":
				if (other.tag != source) {
					Creature c = other.GetComponent<Creature>();
					if (c.currHealth <= damage && other.tag == "Enemy") {
						Destroy(other.GetComponent<Enemy>());
						Destroy(other.GetComponent<Rigidbody2D>());
						other.isTrigger = false;
						other.gameObject.GetComponent<SpriteRenderer>().material.color =  Color.grey;
						Wall w = other.gameObject.AddComponent<Wall>();
						w.hp = 4;
						w.destructible = true;
						other.gameObject.tag = "Wall";
						other.gameObject.layer = 8;
					} else {
						c.takeDamage(damage);
						ApplyKnockback(other);
					}
					Destroy(gameObject);
				}
				break;
		}
    }
}

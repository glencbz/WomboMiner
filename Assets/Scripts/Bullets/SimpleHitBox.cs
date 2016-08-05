using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleHitBox : Bullet {

	private bool hit = false;

	private HashSet<Collider2D> others;
	private Collider2D[] marked;

	void Start() {
		others = new HashSet<Collider2D>();
	}
	public override void hitScan() {
		//Remove all invalid colliders first before iterating
		others.RemoveWhere(c => !c);
		foreach (Collider2D c in others) {
			if (c.tag == "Enemy") {
				c.GetComponent<Enemy>().takeDamage(damage);
			}
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		others.Add(other);
	}

	void OnTriggerExit2D(Collider2D other) {
		others.Remove(other);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleHitBox : Bullet {

	protected bool hit = false;

	protected HashSet<Collider2D> others;
	protected Collider2D[] marked;

	void Start() {
		others = new HashSet<Collider2D>();
	}
	public override void hitScan() {
		//Remove all invalid colliders first before iterating
		others.RemoveWhere(c => !c);
		foreach (Collider2D c in others) {
			if (c.tag == "Enemy") {
				c.GetComponent<Enemy>().takeDamage(damage);
				ApplyKnockback(c);
			}
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		others.Add(other);
	}

	void OnTriggerExit2D(Collider2D other) {
		others.Remove(other);
	}

	protected override void ApplyKnockback(Collider2D other) {
		Vector2 knockback_dir = (transform.position - transform.parent.position).normalized;
		other.GetComponent<Rigidbody2D>().AddForce(knockback_dir * knockback, ForceMode2D.Impulse);
	}
}

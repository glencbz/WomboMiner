using UnityEngine;
using System.Collections;

public class SimpleHitBox : Bullet {

	private bool hit = false;
	public override void hitScan() {
		hit = true;
	}
	void OnTriggerStay2D(Collider2D other) {
		if (hit) {
			//Deal dmg
		}
		hit = false;
	}
}

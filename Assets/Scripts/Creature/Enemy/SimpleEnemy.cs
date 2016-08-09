using UnityEngine;
using System.Collections;

public class SimpleEnemy : Enemy {
	public int touch_damage;
	
	// Use this for initialization
	new void Start () {
		base.Start ();
	}

	void Update() {
		base.Update ();

		if (!this.isActive) {
			return;
		}
	}

	public override void contactPlayer(Collider2D other) {
		other.GetComponent<Player>().takeDamage(3);
	}
}

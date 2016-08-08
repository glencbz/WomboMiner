using UnityEngine;
using System.Collections;

public class SimpleEnemy : Enemy {
	public int touch_damage;
	
	// Update is called once per frame
	void Update () {
		base.Start ();
	}

	public override void contactPlayer(Collider2D other) {
		Debug.Log("TOUCHED");
		other.GetComponent<Player>().takeDamage(3);
	}
}

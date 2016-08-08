using UnityEngine;
using System.Collections;

public class SimpleEnemy : Enemy {
	public int touch_damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void contactPlayer(Collider2D other) {
		Debug.Log("TOUCHED");
		other.GetComponent<Player>().takeDamage(3);
	}
}

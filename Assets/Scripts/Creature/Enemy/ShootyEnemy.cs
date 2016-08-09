using UnityEngine;
using System.Collections;

public class ShootyEnemy : Enemy {
	public int touch_damage;
	public Weapon weapon;
	public GameObject player;
	public Bullet bullet;

	// Use this for initialization
	new void Start () {
		base.Start ();
		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void Update () {
		base.Update ();
		weapon.FireBullet (player.transform.position);
	}

	public override void contactPlayer(Collider2D other) {
		Debug.Log("TOUCHED");
		other.GetComponent<Player>().takeDamage(3);
	}
}

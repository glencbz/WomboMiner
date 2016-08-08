using UnityEngine;
using System.Collections;

public class ShootyEnemy : Enemy {
	public int touch_damage;
	public Weapon weapon;
	public GameObject player;
	public Bullet bullet;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		weapon.FireBullet (player.transform.position);
	}

	public override void contactPlayer(Collider2D other) {
		Debug.Log("TOUCHED");
		other.GetComponent<Player>().takeDamage(3);
	}
}

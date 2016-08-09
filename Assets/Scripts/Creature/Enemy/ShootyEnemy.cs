using UnityEngine;
using System.Collections;

public class ShootyEnemy : Enemy {
	public int touch_damage;
	public Weapon weapon;
	public Bullet bullet;

	// Use this for initialization
	new void Start () {
		base.Start ();
	}

	// Update is called once per frame
	void Update () {
		base.Update ();

		if (!this.isActive) {
			return;
		}

		if (this.isAggroed) {
			weapon.FireBullet (this.player.transform.position);			
		}

	}

	public override void contactPlayer(Collider2D other) {
		Debug.Log("TOUCHED");
		other.GetComponent<Player>().takeDamage(3);
	}
}

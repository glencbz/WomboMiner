using UnityEngine;
using System.Collections;

public class SimpleBullet : Bullet {

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            Enemy e = other.GetComponent<Enemy>();
			e.dealDamage(damage);

		}
    }
}

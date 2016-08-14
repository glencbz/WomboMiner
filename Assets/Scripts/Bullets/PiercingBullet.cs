using UnityEngine;
using System.Collections;

// Generic bullet that travels and does not stop. Has a limited duration
public class PiercingBullet : Bullet {

	public float duration = 2f;

	void Start(){
		Invoke("DestroySelf", duration);
	}

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            Enemy e = other.GetComponent<Enemy>();
			e.takeDamage(damage);
		}
    }

}

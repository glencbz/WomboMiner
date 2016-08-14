using UnityEngine;
using System.Collections;

public class Fist : Weapon {
	protected override void GenerateBullet(Vector3 mousePos){
		Bullet newBullet = GetComponentInChildren<Bullet>();
		newBullet.InitialFire(transform, mousePos);
        Animator a = GetComponentInChildren<Animator>();
        if (a) {a.SetTrigger("fire"); }
	}
}

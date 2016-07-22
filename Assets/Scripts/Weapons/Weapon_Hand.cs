using UnityEngine;
using System.Collections;

public class Weapon_Hand : Weapon {

	protected override void GenerateBullet(Vector3 mousePos){
		Bullet newBullet = GetComponentInChildren<Bullet>();
		newBullet.InitialFire(transform, mousePos);
        Animator a = GetComponentInChildren<Animator>();
        if (a) {a.SetTrigger("fire"); }
	}
}

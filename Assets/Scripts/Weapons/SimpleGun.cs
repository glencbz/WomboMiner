using UnityEngine;
using System.Collections;

public class SimpleGun : Weapon {
    protected override void GenerateBullet(Vector3 mousePos){
		base.GenerateBullet(mousePos);
        Animator a = GetComponentInChildren<Animator>();
        if (a) {a.SetTrigger("fire"); }
	}
}

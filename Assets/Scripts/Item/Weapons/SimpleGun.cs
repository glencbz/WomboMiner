using UnityEngine;
using System.Collections;

// Most basic gun behaviour, create one bullet in the target direction every time the mouse is clicked
public class SimpleGun : Weapon {
    protected override void GenerateBullet(Vector3 mousePos){
		base.GenerateBullet(mousePos);
        Animator a = GetComponentInChildren<Animator>();
        if (a) {a.SetTrigger("fire"); }
	}
}

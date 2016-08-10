using UnityEngine;
using System.Collections;

public class Longinus : Weapon {

	
	protected override void Update () {
		//Update cooldown
		if (cooldownStatus > 0) {
			cooldownStatus -= Time.deltaTime;
			if (cooldownStatus < 0) { 
				cooldownStatus = 0;
				sr.enabled = true;
				transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
	public override void FireBullet(Vector3 direction) {
		if (cooldownStatus <= 0){
			cooldownStatus = cooldown;
			GenerateBullet(direction);
			sr.enabled = false;
			transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		}
	}
}

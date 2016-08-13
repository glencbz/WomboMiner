using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
	public string side;
	public Weapon weapon = null;
	public bool checkEmpty() {
		if (!weapon) {
			GetComponent<SpriteRenderer>().enabled = true;
			return true;
		} else {
			GetComponent<SpriteRenderer>().enabled = false;
			return false;
		}
	}
	
	public bool Equip(Weapon w) {
		bool dropped = Drop();
		if (dropped) {
			this.weapon = w;
			w.gameObject.transform.SetParent(this.transform, false);
			w.gameObject.transform.localPosition = Vector3.zero;
			checkEmpty();
			HUD.Announce(side, weapon.name, weapon.desc, -1);
			HUD.Announce("mid", weapon.name, weapon.desc, 2f);
			return true;
		} else {
			return false;
		}
	}

	public bool Drop() {
		if (weapon)  {
			bool dropped = weapon.Drop();
			if (dropped) weapon = null;
			else return false;
			return true;
		}
		else return true;
	}

	public void updateSprite(float angle) {
		if (!weapon) return;
		else weapon.updateSprite(angle);
	}


}

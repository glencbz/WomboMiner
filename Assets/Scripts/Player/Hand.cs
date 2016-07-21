using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {


	public Weapon weapon = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool checkEmpty() {
		if (!weapon) {
			GetComponent<SpriteRenderer>().enabled = true;
			return true;
		} else {
			GetComponent<SpriteRenderer>().enabled = false;
			return false;
		}
	}

	public bool Drop() {
		if (weapon) return weapon.Drop();
		else return true;
	}

	public void updateSprite(float angle) {
		if (!weapon) return;
		else weapon.updateSprite(angle);
	}

	public bool Equip(Weapon w) {
		bool dropped = Drop();
		if (dropped) {
			this.weapon = w;
			w.gameObject.transform.SetParent(this.transform, false);
			w.gameObject.transform.localPosition = Vector3.zero;
			checkEmpty();
			return true;
		} else {
			return false;
		}
	}
}

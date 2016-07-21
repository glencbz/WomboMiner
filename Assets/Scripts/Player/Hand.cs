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
}

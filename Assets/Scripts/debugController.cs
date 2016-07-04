using UnityEngine;
using System.Collections;

public class debugController : MonoBehaviour {

	public GameObject HUD;
	
	// Update is called once per frame
	void Update () {
		//Enable/Disable HUD
		if (Input.GetKeyDown(KeyCode.Keypad0)) {
			if (HUD.active) {HUD.SetActive(false);} else {HUD.SetActive(true);}
		}
	}
}

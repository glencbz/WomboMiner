using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Announcer : MonoBehaviour {

	public Text title;
	public Text description;

	private bool show = false;
	// Use this for initialization
	void Start () {
		title = transform.GetChild(0).GetComponent<Text>();
		description = transform.GetChild(1).GetComponent<Text>();

		title.enabled = false;
		description.enabled = false;
	}
	
	//Pass the strings into the respective fields
	void updateText(string t = "Junk", string desc = "Nothing interesting about it") {
		title.text = t;
		description.text = desc;
	}

	//Method to show text
	void showText() {
		title.enabled = true;
		description.enabled = true;
	}
 }

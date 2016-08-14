using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Announcer : MonoBehaviour {
	/*
	A wall of text consisting of a Title and Description.
	Use updateText to change the respective text fields.
	showText shows the text for a certain amount of time. Use delay = 0 to show it indefinitely.
	use hideText to hide the text after showing it indefinitely.
	*/
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
	public void updateText(string t = "Weird Stuff", string desc = "What is this???") {
		if (t != null) { title.text = t;}
		if (desc != null) {description.text = desc;}
		
	}

	//Method to show text. Delay of 0 will not invoke switching text off
	public void showText(float delay = 0) {
		title.enabled = true;
		description.enabled = true;
		if (delay != 0) {
			Invoke("hideText", delay);
		}

	}

	public void hideText() {
		title.enabled = false;
		description.enabled = false;
	}
 }

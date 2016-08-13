using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	public static Announcer mainAnnouncer;
	public static Announcer leftAnnouncer;
	public static Announcer rightAnnouncer;

	// Use this for initialization
	void Start () {
		this.GetComponent<Canvas>().enabled = true;
		var announcers = this.transform.GetChild(3);
		mainAnnouncer = announcers.GetChild(0).GetComponent<Announcer>();
		leftAnnouncer = announcers.GetChild(1).GetComponent<Announcer>();
		rightAnnouncer = announcers.GetChild(2).GetComponent<Announcer>();
	}


	/*Announcer static method. 
	String side: left, right or mid. Determines which announcer to Use
	String t: Title text. default option does not change existing text
	String desc: Description text. default option does not change existing text
	float delay: How long to show announcer text. default (0) does not show. use 0 to show indefinitely, -1 to not show
	*/
	public static void Announce(string side, string t=null, string desc=null, float delay=0) {
		switch(side) {
			case "left":
				leftAnnouncer.updateText(t, desc);
				if (delay != -1) {leftAnnouncer.showText(delay);}
				break;
			case "right":
				rightAnnouncer.updateText(t, desc);
				if (delay != -1) {rightAnnouncer.showText(delay);}
				break;
			case "mid":
				mainAnnouncer.updateText(t, desc);
				if (delay != -1) {mainAnnouncer.showText(delay);}
				break;
		}
	}
}

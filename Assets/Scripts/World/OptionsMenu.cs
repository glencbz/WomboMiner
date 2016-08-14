using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {
	private Canvas c;
	private Canvas hud;
	// Use this for initialization
	void Start () {
		//gameObject.SetActive(false);
		c = this.gameObject.GetComponent<Canvas>();
		hud = GameObject.Find("HUD").GetComponent<Canvas>();

		c.enabled = false;
	}

	public void Toggle() {
		if (c.enabled) {
			// resume
			GameManager.instance.ResumeGame();
			c.enabled = false;
			hud.enabled = true;

		} else {
			// pause
			GameManager.instance.PauseGame();
			c.enabled = true;
			hud.enabled = false;
		}

	}

	public void ResumeGame() {
		Toggle();
	}
	public void RestartGame() {
		GameManager.instance.RestartGame ();
	}

	public void ExitGame() {
		GameManager.instance.ExitGame ();
	}
}

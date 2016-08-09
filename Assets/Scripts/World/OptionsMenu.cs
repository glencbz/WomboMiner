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
			c.enabled = false;
			hud.enabled = true;
		} else {
			c.enabled = true;
			hud.enabled = false;
		}

	}

	public void ResumeGame() {
		Toggle();
	}
	public void RestartGame() {
		int sceneNum = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNum);
	}

	public void ExitGame() {
		GameManager.instance.isDungeon = false;
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}

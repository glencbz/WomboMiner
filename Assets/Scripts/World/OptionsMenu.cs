using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ResumeGame() {
		this.gameObject.GetComponent<Canvas>().enabled = false;
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

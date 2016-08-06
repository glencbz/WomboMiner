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
		this.gameObject.SetActive(false);
	}
	public void RestartGame() {
		int sceneNum = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNum);
	}
}

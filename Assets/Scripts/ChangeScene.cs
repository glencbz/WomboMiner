using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public GameObject loadingScreen = null;
	public void nextScene(string scene){
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}

	public void nextScene(int scene){
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}
}

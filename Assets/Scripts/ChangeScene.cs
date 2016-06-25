using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
public void nextScene(string scene){
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}
}

using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

	public string clickerSceneName;
	public Canvas optionMenu;

	// Use this for initialization
	void Start () {
		optionMenu = optionMenu.GetComponent<Canvas> ();
		optionMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeClicker(){
		UnityEngine.SceneManagement.SceneManager.LoadScene(clickerSceneName);
	}

	public void OpenOptionMenu(){
		optionMenu.enabled = true;
	}

	public void CloseOptionMenu(){
		optionMenu.enabled = false;
	}
}

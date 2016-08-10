using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public Text loadingText;

	private Canvas c;

	private bool loading = false;
	// Use this for initialization
	void Start () {
		loadingText = GetComponentInChildren<Text>();
		c = GetComponent<Canvas>();
		c.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (loading) {
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
		}
		
	}

	public void LoadScene(int num) {
		loading = true;
		c.enabled = true;
		// if is dungeon, set is dungeon to true
		if (num == 1) {
			GameManager.instance.isDungeon = true;
		}

		StartCoroutine(load(num));

	}

	public void LoadScene(string name) {
		int i = UnityEngine.SceneManagement.SceneManager.sceneCount;
		for (int j=0;j<i;j++) {
			Debug.Log(UnityEngine.SceneManagement.SceneManager.GetSceneAt(j).name);
		}
		Debug.Log(name);
		int num = UnityEngine.SceneManagement.SceneManager.GetSceneByName(name).buildIndex;
		Debug.Log(num);
		LoadScene(num);

	}

	IEnumerator load(int num) {
		yield return new WaitForSeconds(1);
		AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(num);

		while(!async.isDone) {
			yield return null;
		}
		
	}
}

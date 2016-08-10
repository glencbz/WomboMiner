using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScore : MonoBehaviour {
	void Awake () {
		int highScore = PlayerPrefs.GetInt ("highScore");
		this.GetComponent<Text> ().text = "High Score: " + highScore.ToString ();
	}
}

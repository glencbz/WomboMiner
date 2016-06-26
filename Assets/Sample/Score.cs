using UnityEngine;
using System.Collections;

[System.Serializable]
public class Score  {
	public static Score current;
	public int score = 0;
	public int highScore = 0;

	public void LoadScore() {
		current = this;
		bool load = SaveLoad.Load();
		if (load) {
			this.highScore = SaveLoad.saveFile.highScore;
		}
	}
}

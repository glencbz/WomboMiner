using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour {

	private Score data;
	public Text highScoreText;
	public Text Scoretext;
	public int startTime;
    public Text Timetext;
    public Button b;

    private float secondCount;
    private bool end = false;

	void Start() {
		//Init data and load
		data = new Score();
		data.LoadScore();

		//Init all text
		Scoretext.text = "Score:\n" + data.score.ToString();
		highScoreText.text = "High Score:\n" + data.highScore.ToString();
		Timetext.text = "Time Left: \n" + startTime.ToString();
	}

	void Update () {
		//Update timer
        secondCount += Time.deltaTime;
        if (secondCount > 1.0f && startTime > 0)
        {
            startTime -= 1;
            secondCount -= 1.0f;
        }
        Timetext.text = "Time Left: \n" + startTime.ToString();

		//End game if timer reaches 0
        if (startTime == 0 && end == false) {
            endGame();
            end = true;

        }
    }

	public void scoreUp() {
		data.score += 1;
		updateScore();
	}

	public void setScore(int newScore) {
		data.score = newScore;
		updateScore();
	}
	
	void updateScore() {
		Scoretext.text = "Score:\n" + data.score.ToString();
		highScoreText.text = "High Score:\n" + data.highScore.ToString();
	}

	void saveScore() {
		//Save only if new score is higher
		if (data.score > data.highScore) {
			data.highScore = data.score;
			updateScore();
			SaveLoad.Save();
		}
	}
	void endGame() {
        saveScore();
    	b.interactable = false;
    }
}

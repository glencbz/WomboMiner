using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controller : MonoBehaviour {

	private Score data;
	public int startTime;
	private Text highScoreText;
	private Text Scoretext;
    private Text Timetext;
    private Button scoreButton;
	private Button resetButton;

	private int timeLeft;
    private float secondCount;
    private bool end = false;

	void Start() {
		//Find all UI elements
		highScoreText = GameObject.Find("High Score Text").GetComponent<Text>();
		Scoretext = GameObject.Find("Score Text").GetComponent<Text>();	
		Timetext = GameObject.Find("Timer Text").GetComponent<Text>();
		scoreButton = GameObject.Find("ScoreButton").GetComponent<Button>();
		resetButton = GameObject.Find("Reset Button").GetComponent<Button>();

		//Init data and load
		data = new Score();
		data.LoadScore();

		//Init timeLeft
		timeLeft = startTime;
		//Init all text
		Scoretext.text = "Score:\n" + data.score.ToString();
		highScoreText.text = "High Score:\n" + data.highScore.ToString();
		Timetext.text = "Time Left: \n" + timeLeft.ToString();

		//Hide reset Button
		resetButton.gameObject.SetActive(false);
	}

	void Update () {
		//Update timer
        secondCount += Time.deltaTime;
        if (secondCount > 1.0f && timeLeft > 0)
        {
            timeLeft -= 1;
            secondCount -= 1.0f;
			Timetext.text = "Time Left: \n" + timeLeft.ToString();
        }
        

		//End game if timer reaches 0
        if (timeLeft == 0 && end == false) {
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
			Timetext.text = "New High Score!";
			SaveLoad.Save();
		}
	}
	void endGame() {
        saveScore();
    	scoreButton.enabled = false;
		resetButton.gameObject.SetActive(true);
    }

	public void resetGame() {
		data.score = 0;
		timeLeft = startTime;
		secondCount = 0;
		end = false;
		Timetext.text = "Time Left: \n" + timeLeft.ToString();
		updateScore();
		scoreButton.enabled = true;
		resetButton.gameObject.SetActive(false);

	}
}

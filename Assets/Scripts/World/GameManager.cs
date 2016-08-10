using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
	public float turnDelay = 0.1f;							//Delay between each Player turn.
	public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
	
	public bool isDungeon = false;
	public bool isSurvival = false;
	public GameObject options;
	private Text levelText;									//Text to display current level number.
	private Canvas deathScreen;
	private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
	private Text scoreText;
	private Text deathScore;
	private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
	private int score;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);				
		}

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		
		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();

		if (options == null) {
			options = GameObject.Find("Options Menu");
		}
	}

	void Update() {
		if (!isDungeon) {
			return;
		}
			
		if (Input.GetKeyDown(KeyCode.Escape)) {
			options.GetComponent<OptionsMenu>().Toggle();
		}
	}

	void OnLevelWasLoaded(int index) {
		options = GameObject.Find("Options Menu");
		if (isDungeon) {
			InitGame();
		}
	}

	void InitGame() {
		this.score = 0;
		levelImage = GameObject.Find("Opening Screen");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		this.scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();

		deathScreen = GameObject.Find ("DeathScreen").GetComponent<Canvas>();
		deathScore = GameObject.Find ("DeathScore").GetComponent<Text> ();
		deathScreen.enabled = false;

		levelImage.GetComponent<Canvas>().enabled = true;

		//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
		Invoke("HideLevelImage", levelStartDelay);

		if (!this.isSurvival) {
			levelText.text = "GOOD LUCK";
			this.GetComponent<BoardCreator> ().Setup ();	
		} else {
			levelText.text = "SURVIVE";
			this.GetComponent<SurvivalBoardCreator> ().Setup ();
		}
	}

	void HideLevelImage() {
		// hide the overlay for SURVIVE or CLASSIC
		levelImage.SetActive(false);
	}

	public void GameOver() {
		levelText.text = "GAME OVER";
		deathScreen.enabled = true;
		deathScore.text = "Your Score: " + this.score.ToString ();
	}

	public void GoBackToMainMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		GameManager.instance.isDungeon = false;
	}

	public void OnEnemyKilled(int enemyScore) {
		this.score += enemyScore;
		this.scoreText.text = this.score.ToString();
	}
}

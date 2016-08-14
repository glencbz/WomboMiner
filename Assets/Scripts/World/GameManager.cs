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
	private Canvas deathScreen;
	private GameObject openingScreen;
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
		openingScreen = GameObject.Find("Opening Screen");

		this.scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();

		deathScreen = GameObject.Find ("DeathScreen").GetComponent<Canvas>();
		deathScore = GameObject.Find ("DeathScore").GetComponent<Text> ();
		deathScreen.enabled = false;

		//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
		Invoke("HideOpeningScreen", levelStartDelay);

		if (!this.isSurvival) {
			this.GetComponent<BoardCreator> ().Setup ();	
		} else {
			this.GetComponent<SurvivalBoardCreator> ().Setup ();
			this.GetComponent<EnemySpawner> ().onSpawner ();
		}
	}

	void HideOpeningScreen() {
		// hide the overlay for SURVIVE or CLASSIC
		openingScreen.SetActive(false);
	}

	public void GameOver() {

		if(this.isSurvival) {
			this.GetComponent<EnemySpawner> ().offSpawner ();
		}

		deathScreen.enabled = true;
		deathScore.text = "Your Score: " + this.score.ToString ();


		int highScore = PlayerPrefs.GetInt ("highScore");
		if (this.score > highScore) {
			PlayerPrefs.SetInt ("highScore", this.score);
		}
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

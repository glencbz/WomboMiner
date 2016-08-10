using UnityEngine;
using System.Collections;

// EnemySpawner script attached to same object as the survival board creator script
public class EnemySpawner : MonoBehaviour
{
	public Enemy[] spawnedEnemies;

	public float[] xrange = {0,30};
	public float[] yrange = {0,30};

	private IntRange xGenerator;
	private IntRange yGenerator;
	private IntRange enemyGenerator;

	public float period = 1;
	public float wavePeriod = .2f;
	public float spawnStatus = 0;
	public bool inWave = false;

	public float probability = 0.2f;

	private bool active = false;

	// Use this for initialization
	void Start ()
	{
		SurvivalBoardCreator boardCreator = GetComponent<SurvivalBoardCreator>();
		xrange[1] = boardCreator.columns;
		yrange[1] = boardCreator.rows;
		xGenerator = new IntRange((int) xrange[0],(int) xrange[1]);
		yGenerator = new IntRange((int)yrange[0],(int) yrange[1]);
		enemyGenerator = new IntRange(0, spawnedEnemies.Length);
		spawnStatus = period;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!active) {
			return;
		}
		spawnStatus -= Time.deltaTime;
		if (spawnStatus <= 0) {
			SpawnEnemy();
			if (!inWave)
				spawnStatus = period;
			else
				spawnStatus = wavePeriod;
		}
	}

	private void SpawnEnemy(){
		int xCoord = xGenerator.Random;
		int yCoord = yGenerator.Random;
		float value = Random.Range (0, 1f);
		if (value <= probability) {
			Instantiate (spawnedEnemies [0], new Vector3 (xCoord, yCoord, 0), Quaternion.identity);
		} else {
			Instantiate (spawnedEnemies [1], new Vector3 (xCoord, yCoord, 0), Quaternion.identity);
		}
	}

	public void onSpawner() {
		active = true;
	}

	public void offSpawner() {
		active = false;
	}
}


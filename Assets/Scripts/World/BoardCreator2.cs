using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator2 : MonoBehaviour
{
	// The type of tile that will be laid in a specific position.
	public enum TileType
	{
		Wall, Floor,
	}

	public int enemyCount;
	public float probability;
	public int columns = 200;                                 // The number of columns on the board (how wide it will be).
	public int rows = 200;                                    // The number of rows on the board (how tall it will be).
	public IntRange numRooms = new IntRange (15, 20);         // The range of the number of rooms there can be.
	public IntRange roomWidth = new IntRange (20, 20);         // The range of widths rooms can have.
	public IntRange roomHeight = new IntRange (20, 20);        // The range of heights rooms can have.
	public IntRange bossRoomWidth = new IntRange (30, 30);         // The range of widths boss room can have.
	public IntRange bossRoomHeight = new IntRange (30, 30);        // The range of heights boss room can have.
	public GameObject[] floorTiles;                           // An array of floor tile prefabs.
	public GameObject[] wallTiles;                            // An array of wall tile prefabs.
	public GameObject[] outerWallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] weaponTiles;
	private GameObject player;

	public GameObject[] players;

	private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
	private Room[] rooms;
	private Room[] currentRooms;
	private Corridor[] corridors;                             // All the corridors that connect the rooms.
	private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.


	void Awake () {
		// awake is called everytime the scene is loaded if the parent gameObject is not destroyed
		this.Setup ();
	}

	void Setup() {
		// Create the board holder.
		boardHolder = new GameObject("BoardHolder");

		SetupTilesArray ();	
		player = GameObject.FindGameObjectWithTag("Player");


//		CreateRoomsAndCorridors ();

//		SetTilesValuesForRooms ();
//		SetTilesValuesForCorridors ();

		InstantiateTiles ();
		InstantiateOuterWalls ();
//		InstantiateEnemies ();
		InstantiateWeapons ();

	}


	void SetupTilesArray ()
	{
		// Set the tiles jagged array to the correct width.
		tiles = new TileType[columns][];

		// Go through all the tile arrays...
		for (int i = 0; i < tiles.Length; i++)
		{
			// ... and set each tile array is the correct height.
			tiles[i] = new TileType[rows];
		}

		for(int j=0; j<tiles.Length; j++) {
			for (int k=0; k<tiles[j].Length; k++) {
				tiles [j] [k] = TileType.Floor;
			}
		}
	}
		
	void InstantiateTiles ()
	{
		// Go through all the tiles in the jagged array...
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{
				// ... and instantiate a floor tile for it.
				InstantiateFromArray (floorTiles, i, j);
				Debug.Log (new Vector2 (i, j));
				// If the tile type is Wall...
				if (tiles[i][j] == TileType.Wall)
				{
					// ... instantiate a wall over the top.
					InstantiateFromArray (wallTiles, i, j);
				}
			}
		}
	}


	void InstantiateOuterWalls ()
	{
		// The outer walls are one unit left, right, up and down from the board.
		float leftEdgeX = -1f;
		float rightEdgeX = columns + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = rows + 0f;

		// Instantiate both vertical walls (one on each side).
		InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

		// Instantiate both horizontal walls, these are one in left and right from the outer walls.
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
	}


	void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
	{
		// Start the loop at the starting value for Y.
		float currentY = startingY;

		// While the value for Y is less than the end value...
		while (currentY <= endingY)
		{
			// ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
			InstantiateFromArray(outerWallTiles, xCoord, currentY);

			currentY++;
		}
	}


	void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
	{
		// Start the loop at the starting value for X.
		float currentX = startingX;

		// While the value for X is less than the end value...
		while (currentX <= endingX)
		{
			// ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
			InstantiateFromArray (outerWallTiles, currentX, yCoord);

			currentX++;
		}
	}


	void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
	{
		// Create a random index for the array.
		int randomIndex = Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		// Create an instance of the prefab from the random index of the array.
		GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
	}


	void InstantiateEnemies () {

		List<int> roomChoices = new List<int>();

		int minimumEnemies = enemyCount / (rooms.Length-1);

		int currentIndex = 0;
		for (int i =1; i< rooms.Length; i++) {

			for (int j=0; j<minimumEnemies; j++) {
				roomChoices.Add(i);
			}
			currentIndex += minimumEnemies;
		}

		int remainingEnemiesCount = enemyCount - (minimumEnemies * (rooms.Length - 1));
		for (int i=0; i<remainingEnemiesCount;i++) {
			roomChoices.Add(Random.Range (1,rooms.Length-1));
		}


		for (int i = 0; i < enemyCount; i++) {
			//Randomly choose a room
			int roomIndex = roomChoices[Random.Range (0, roomChoices.Count-1)];
			roomChoices.Remove (roomIndex);
			Room chosenRoom = rooms [roomIndex];

			//Get max range for spawning
			int maxX = chosenRoom.xPos + chosenRoom.roomWidth;
			int maxY = chosenRoom.yPos + chosenRoom.roomHeight;

			//Choose enemy
			GameObject tileChoice = enemyTiles[Random.Range (0, enemyTiles.Length)];

			//Randomly choose spawn position within range
			int posX = Random.Range (chosenRoom.xPos, maxX);
			int posY = Random.Range (chosenRoom.yPos, maxY);
			Vector2 randomPosition = new Vector2 (posX, posY);

			//Instantiate enemy
			GameObject enemy = (GameObject)Instantiate (tileChoice, randomPosition, Quaternion.identity);
			// disable enemy ai on start for performance
			enemy.GetComponent<Enemy> ().enabled = false;
		}
	}


	void InstantiateWeapons () {

		for (int i = 0; i < weaponTiles.Length; i++) {


			GameObject weapon = weaponTiles [i];
			Vector2 randomPosition = new Vector2 (0, 0);

			//Instantiate enemy
			Instantiate (weapon, randomPosition, Quaternion.identity);
		}
	}

}
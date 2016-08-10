using System.Collections;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
	// The type of tile that will be laid in a specific position.
	public enum TileType
	{
		Wall, Floor,
	}


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
	public int enemyCount;
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


		CreateRoomsAndCorridors ();

		SetTilesValuesForRooms ();
		SetTilesValuesForCorridors ();

		InstantiateTiles ();
		InstantiateOuterWalls ();
		InstantiateEnemies ();
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
	}


	void CreateRoomsAndCorridors ()
	{
		// Create the rooms array with a random size.
		int normRoom = numRooms.Random;
		rooms = new Room[normRoom+1];

		// There should be one less corridor than there is rooms.
		corridors = new Corridor[rooms.Length - 1];			

		// Create the first room and corridor.
		rooms[0] = new Room ();

		// Setup the first room, there is no previous corridor so we do not use one.
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);


		Vector3 playerPos = new Vector3 (rooms[0].xPos, rooms[0].yPos, 0);
		player.transform.position = playerPos;

		for (int i = 1; i < normRoom; i++) {
			currentRooms = new Room[i];
			for (int j = 0; j< currentRooms.Length; j++){
				currentRooms [j] = rooms [j];
			}
			rooms [i] = new Room ();
			rooms [i].SetupRoom (roomWidth, roomHeight, columns, rows, currentRooms,corridors);
		}
		rooms [normRoom] = new Room ();
		rooms [normRoom].SetupBossRoom (bossRoomWidth, bossRoomHeight, columns, rows, currentRooms,corridors);

	}


	void SetTilesValuesForRooms ()
	{
		// Go through all the rooms...
		for (int i = 0; i < rooms.Length; i++)
		{
			Room currentRoom = rooms[i];

			// ... and for each room go through it's width.
			for (int j = 0; j < currentRoom.roomWidth; j++)
			{
				int xCoord = currentRoom.xPos + j;

				// For each horizontal tile, go up vertically through the room's height.
				for (int k = 0; k < currentRoom.roomHeight; k++)
				{
					int yCoord = currentRoom.yPos + k;
					// The coordinates in the jagged array are based on the room's position and it's width and height.
					tiles[xCoord][yCoord] = TileType.Floor;
				}
			}
		}
	}


	void SetTilesValuesForCorridors ()
	{
		// Go through every corridor...
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor currentCorridor = corridors[i];
			//Debug.Log (currentCorridor.corridorLength);
			// and go through it's length.
			for (int j = 0; j < currentCorridor.corridorLength; j++)
			{
				// Start the coordinates at the start of the corridor.
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;
				// Depending on the direction, add or subtract from the appropriate
				// coordinate based on how far through the length the loop is.
				switch (currentCorridor.direction)
				{
				case Direction.North:
					yCoord += j;
					break;
				case Direction.East:
					xCoord += j;
					break;
				case Direction.South:
					yCoord += j;
					break;
				case Direction.West:
					xCoord += j;
					break;
				}

				// Set the tile at these coordinates to Floor.
				tiles[xCoord][yCoord] = TileType.Floor;
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
		for (int i = 0; i < enemyCount; i++) {
			//Randomly choose a room
			int roomIndex = Random.Range (0, rooms.Length - 1);
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

		Room chosenRoom = rooms [0];

		for (int i = 0; i < weaponTiles.Length; i++) {


			GameObject weapon = weaponTiles [i];

			//Get max range for spawning
			int maxX = chosenRoom.xPos + chosenRoom.roomWidth;
			int maxY = chosenRoom.yPos + chosenRoom.roomHeight;

			//Randomly choose spawn position within range
			int posX = Random.Range (chosenRoom.xPos, maxX);
			int posY = Random.Range (chosenRoom.yPos, maxY);
			Vector2 randomPosition = new Vector2 (posX, posY);

			//Instantiate enemy
			Instantiate (weapon, randomPosition, Quaternion.identity);
		}
	}
//	//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
//	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
//	{
//		//Choose a random number of objects to instantiate within the minimum and maximum limits
//		int objectCount = Random.Range (minimum, maximum+1);
//
//		//Instantiate objects until the randomly chosen limit objectCount is reached
//		for(int i = 0; i < objectCount; i++)
//		{
//			//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
//			Vector3 randomPosition = RandomPosition();
//
//			//Choose a random tile from tileArray and assign it to tileChoice
//			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
//
//			//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
//			Instantiate(tileChoice, randomPosition, Quaternion.identity);
//		}
//	}

//	//RandomPosition returns a random position from our list gridPositions.
//	Vector3 RandomPosition ()
//	{
//		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
//		int randomIndex = Random.Range (0, gridPositions.Count);
//
//		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
//		Vector3 randomPosition = gridPositions[randomIndex];
//
//		//Remove the entry at randomIndex from the list so that it can't be re-used.
//		gridPositions.RemoveAt (randomIndex);
//
//		//Return the randomly selected Vector3 position.
//		return randomPosition;
//	}
}
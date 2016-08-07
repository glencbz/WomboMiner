using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room
{
	public int xPos;                      // The x coordinate of the lower left tile of the room.
	public int yPos;                      // The y coordinate of the lower left tile of the room.
	public int roomWidth;                     // How many tiles wide the room is.
	public int roomHeight;                    // How many tiles high the room is.
	public Direction enteringCorridor;    // The direction of the corridor that is entering this room.
	public Direction adjacentRoomDirection;


	// This is used for the first room.  It does not have a Corridor parameter since there are no corridors yet.
	public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
	{
		// Set a random width and height.
		roomWidth = widthRange.Random;
		roomHeight = heightRange.Random;

		// Set the x and y coordinates so the room is roughly in the middle of the board.


		xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
		yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
	}


	// This is an overload of the SetupRoom function and has a corridor parameter that represents the corridor entering the room.
//	public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
//	{
//		// Set the entering corridor direction.
//		enteringCorridor = corridor.direction;
//
//		// Set random values for width and height.
//		roomWidth = widthRange.Random;
//		roomHeight = heightRange.Random;
//
//		switch (corridor.direction)
//		{
//		// If the corridor entering this room is going north...
//		case Direction.North:
//			// ... the height of the room mustn't go beyond the board so it must be clamped based
//			// on the height of the board (rows) and the end of corridor that leads to the room.
//			roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);
//
//			// The y coordinate of the room must be at the end of the corridor (since the corridor leads to the bottom of the room).
//			yPos = corridor.EndPositionY;
//
//			// The x coordinate can be random but the left-most possibility is no further than the width
//			// and the right-most possibility is that the end of the corridor is at the position of the room.
//			xPos = Random.Range (corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
//
//			// This must be clamped to ensure that the room doesn't go off the board.
//			xPos = Mathf.Clamp (xPos, 0, columns - roomWidth);
//			break;
//		case Direction.East:
//			roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
//			xPos = corridor.EndPositionX;
//
//			yPos = Random.Range (corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
//			yPos = Mathf.Clamp (yPos, 0, rows - roomHeight);
//			break;
//		case Direction.South:
//			roomHeight = Mathf.Clamp (roomHeight, 1, corridor.EndPositionY);
//			yPos = corridor.EndPositionY - roomHeight + 1;
//
//			xPos = Random.Range (corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
//			xPos = Mathf.Clamp (xPos, 0, columns - roomWidth);
//			break;
//		case Direction.West:
//			roomWidth = Mathf.Clamp (roomWidth, 1, corridor.EndPositionX);
//			xPos = corridor.EndPositionX - roomWidth + 1;
//
//			yPos = Random.Range (corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
//			yPos = Mathf.Clamp (yPos, 0, rows - roomHeight);
//			break;
//		}
//	}

	private bool checkOverlap(int X1, int Y1, int W1, int H1, int X2,int Y2,int W2, int H2) {
		if (X1+W1<X2 || X2+W2<X1 || Y1+H1<Y2 || Y2+H2<Y1){
			return false;
		}
		else{
			return true;
		}
	}

	private List<Direction> makeDirectionCheck() {
		return new List<Direction> (new Direction[] {
			Direction.North,
			Direction.South,
			Direction.East,
			Direction.West
		});
	}

	public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows, Room[] currentRooms, Corridor[] corridors)
	{
		int roomIndex = currentRooms.Length - 1;
		Room neighborRoom = currentRooms [roomIndex];

		List<Direction> uncheckedDirections = makeDirectionCheck ();

		bool isNotValid = true;
		bool isOverBoundary;

		roomWidth = widthRange.Random;
		roomHeight = heightRange.Random;

		while (isNotValid) {

			if (uncheckedDirections.Count == 0) {
				roomIndex -= 1;
				neighborRoom = currentRooms [roomIndex];
				uncheckedDirections = makeDirectionCheck ();
			}

			bool isNewDirection = false;
			while (!isNewDirection) {
				adjacentRoomDirection = (Direction)Random.Range (0, 4);
				if (uncheckedDirections.Contains(adjacentRoomDirection)) {
					uncheckedDirections.Remove (adjacentRoomDirection);
					isNewDirection = true;
				} 
			}

			switch (adjacentRoomDirection) {
			case Direction.North:
				yPos = neighborRoom.yPos + neighborRoom.roomHeight + 4;
				xPos = Random.Range (neighborRoom.xPos - (roomWidth - 1), neighborRoom.xPos + (neighborRoom.roomWidth - 1));
				break;
			case Direction.South:
				yPos = neighborRoom.yPos - neighborRoom.roomHeight - 4;
				xPos = Random.Range (neighborRoom.xPos - (roomWidth - 1), neighborRoom.xPos + (neighborRoom.roomWidth - 1));
				break;
			case Direction.East:
				xPos = neighborRoom.xPos + neighborRoom.roomWidth + 4;
				yPos = Random.Range (neighborRoom.yPos - roomHeight + 1, neighborRoom.yPos + (neighborRoom.roomHeight - 1));
				break;
			case Direction.West:
				xPos = neighborRoom.xPos - (roomWidth + 4);
				yPos = Random.Range (neighborRoom.yPos - roomHeight + 1, neighborRoom.yPos + (neighborRoom.roomHeight - 1));
				break;
			}

			bool isOverlapping = true;
			for (int i = 0; i < currentRooms.Length; i++) {
				Room checkingRoom = currentRooms [i];
				isOverlapping = checkOverlap (checkingRoom.xPos, checkingRoom.yPos, checkingRoom.roomWidth, checkingRoom.roomHeight, xPos, yPos, roomWidth, roomHeight);
				if (isOverlapping) {
					break;
				}
			}

			if (xPos + roomWidth > columns || yPos + roomHeight > rows || xPos < 0 || yPos < 0 ){
				isOverBoundary = true;
			} else {
				isOverBoundary = false;
			}

			if (!isOverlapping && !isOverBoundary) {
				isNotValid = false;
				corridors [currentRooms.Length - 1] = new Corridor ();
				corridors [currentRooms.Length - 1].SetupCorridor (neighborRoom, this, adjacentRoomDirection);
			}
					
		}

	}
}
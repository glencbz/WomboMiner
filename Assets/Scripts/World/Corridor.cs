using UnityEngine;

// Enum to specify the direction is heading.
public enum Direction
{
	North, East, South, West,
}

public class Corridor
{
	public int startXPos;         // The x coordinate for the start of the corridor.
	public int startYPos;         // The y coordinate for the start of the corridor.
	public int corridorLength;            // How many units long the corridor is.
	public Direction direction;   // Which direction the corridor is heading from it's room.


	// Get the end position of the corridor based on it's start position and which direction it's heading.
	public int EndPositionX
	{
		get
		{
			if (direction == Direction.North || direction == Direction.South)
				return startXPos;
			if (direction == Direction.East)
				return startXPos + corridorLength - 1;
			return startXPos - corridorLength + 1;
		}
	}


	public int EndPositionY
	{
		get
		{
			if (direction == Direction.East || direction == Direction.West)
				return startYPos;
			if (direction == Direction.North)
				return startYPos + corridorLength - 1;
			return startYPos - corridorLength + 1;
		}
	}

	public void SetupCorridor (Room room1, Room room2, Direction direction) {
		int min;
		int max;

		switch (direction) {
		case Direction.North:
			this.direction = Direction.North;
			this.corridorLength = room2.yPos - (room1.yPos + room1.roomHeight);
			this.startYPos = room1.yPos + room1.roomHeight;
			min = Mathf.Max (room1.xPos, room2.xPos);
			min = Mathf.Max (room1.xPos, room2.xPos);
			max = Mathf.Min (room1.xPos + room1.roomWidth, room2.xPos + room2.roomWidth);
			this.startXPos = Random.Range (min, max);
			break;
		case Direction.South:
			this.direction = Direction.South;
			this.corridorLength = room1.yPos - (room2.yPos + room2.roomHeight);
			this.startYPos = room2.yPos + room2.roomHeight;
			min = Mathf.Max (room1.xPos, room2.xPos);
			max = Mathf.Min (room1.xPos + room1.roomWidth, room2.xPos + room2.roomWidth);
			this.startXPos = Random.Range (min, max);
			break;
		case Direction.East:
			this.direction = Direction.East;
			this.corridorLength = room2.xPos - (room1.xPos + room1.roomWidth);
			this.startXPos = room1.xPos + room1.roomWidth;
			min = Mathf.Max (room1.yPos, room2.yPos);
			max = Mathf.Min (room1.yPos + room1.roomHeight, room2.yPos + room2.roomHeight);
			this.startYPos = Random.Range (min, max);
			break;
		case Direction.West:
			this.direction = Direction.West;
			this.corridorLength = room1.xPos - (room2.xPos + room2.roomWidth);
			this.startXPos = room2.xPos + room2.roomWidth;
			min = Mathf.Max (room1.yPos, room2.yPos);
			max = Mathf.Min (room1.yPos + room1.roomHeight, room2.yPos + room2.roomHeight);
			this.startYPos = Random.Range (min, max);
			break;
		}

	}
		
}
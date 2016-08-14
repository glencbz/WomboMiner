using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Graph object to do generic graph search
 * 
 * Used for enemy pathfinding
 **/
class Graph {
	private Node startNode;

	public Graph(Vector2 start, Vector2 end, HashSet<Vector2>obstacles) {
		this.startNode = new Node (start, end, obstacles, 0, null);
	}

	public List<Vector2> ShortestPath(int levelLimit) {
		Node endNode = this.BreadthFirstSearch (levelLimit);

		// no path found, return empty list
		if (endNode == null) {
			return new List<Vector2>();
		}
		return endNode.GetPath ();
	}

	// returns null if no path is found within the level limit
	private Node BreadthFirstSearch(int levelLimit) {
		Queue queue = new Queue();
		HashSet<Vector2> explored = new HashSet<Vector2> ();

		queue.Enqueue (this.startNode);

		while (queue.Count > 0) {
			Node current = (Node)queue.Dequeue (); 

			if (current.AtGoal()) {
				return current;
			}

			if (explored.Contains(current.position)) {
				continue;
			}

			if (current.LevelLimitExceeded(levelLimit)) {
				continue;
			}

			explored.Add (current.position);

			foreach(Node neighbour in current.GetNeighbours()) {
				queue.Enqueue (neighbour);
			}
		}
		return null;
	}
}

class Node {
	private static Vector2[] directions = new Vector2[]{
		new Vector2(0, 1.0f),
		new Vector2(1.0f, 0),
		new Vector2(-1.0f, 0),
		new Vector2(0, -1.0f)
	};

	public Vector2 position;
	private Vector2 goalPosition;
	private HashSet<Vector2> obstacles;
	private int level;
	private Node previous;

	public Node(Vector2 position, Vector2 goalPosition, HashSet<Vector2> obstacles, int level, Node previous) {
		this.position = position;
		this.goalPosition = goalPosition;
		this.obstacles = obstacles;
		this.level = level;
		this.previous = previous;
	}

	public List<Node> GetNeighbours() {
		List<Node> neighbours = new List<Node> ();

		foreach(Vector2 direction in Node.directions) {
			Vector2 newPosition = direction + this.position;
			if (this.obstacles.Contains(newPosition)) {
				continue;
			}
			neighbours.Add (new Node(newPosition, this.goalPosition, this.obstacles, this.level + 1, this));		
		}

		return neighbours;
	}

	public bool AtGoal() {
		return this.position == this.goalPosition;
	}

	public bool LevelLimitExceeded(int levelLimit) {
		return this.level > levelLimit;
	}

	public List<Vector2> GetPath() {
		Node current = this;
		List<Vector2> path = new List<Vector2> ();

		while (current != null) {
			path.Add (current.position);
			current = current.previous;
		}

		path.Reverse();

		// remove first element from the list, which is the current position of the starting object
		path.RemoveAt (0);
		return path;
	}
}
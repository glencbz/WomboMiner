﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Usage:
 * 
 * Drag script over enemy GameObject
 * 
 * Inspector Variables:
 * 
 * Player: The player GameObject where the enemy will try to attack
 * LayerMask set to BlockingLayer, or the layer objects are present that the enemy is supposed to avoid 
 * 
 **/
public class EnemyMove : MonoBehaviour {

	public GameObject player;

	// set to BlockingLayer in the inspector plz
	public LayerMask layerMask;

	// how deep to do graph search
	private int GRAPH_SEARCH_LIMIT = 10;
	private float moveSpeed;
	private float aggroDistance;

	void Start () {
		Enemy currentEnemy = GetComponent<Enemy> ();
		this.moveSpeed = currentEnemy.moveScale;
		this.aggroDistance = currentEnemy.aggroDistance;
	}

	bool PlayerIsNear() {
		float distance = Vector2.Distance (this.transform.position, this.player.transform.position);
		return distance <= this.aggroDistance;
	}

	bool ClearPathToPlayer() {
		Vector2 dirToObject = this.player.transform.position - this.transform.position;
		RaycastHit2D hit = Physics2D.Raycast (this.transform.position, dirToObject, Mathf.Infinity, layerMask.value);
		// nothing hit means there's a clear path because the player is not on the blocking layer
		return hit.collider == null;
	}

	// Update is called once per frame
	void Update () {
		this.MoveToPlayer ();
	}

	void MoveTowardsPlayerDirectly() {
		this.MoveTowards (this.player.transform.position);
	}

	void MoveTowards(Vector2 position) {
		this.transform.position = Vector2.MoveTowards(this.transform.position, position, this.moveSpeed);
	}

	void MoveToPlayer() {
		// check if player is near (aggro)
		if (!this.PlayerIsNear()) {
			return;
		}

		// if not thing in the way, just move to the player
		if (this.ClearPathToPlayer()) {
			this.MoveTowardsPlayerDirectly ();
			return;
		}

		// get list of coordinates for blocking objects
		HashSet<Vector2> obstacles = this.GetObstacles();

		// do bfs and find a path towards the player
		Graph graph = new Graph (EnemyMove.RoundVector(this.transform.position), EnemyMove.RoundVector(this.player.transform.position), obstacles);
		List<Vector2> shortestPath = graph.ShortestPath (GRAPH_SEARCH_LIMIT);

		if (shortestPath.Count < 1) {
			// workaround for the bug when the player is too close and somehow the ray cast in 
			// ClearPathToPlayer() thinks that there is no clear path, and we end up with a shortest path of length 0

			// to replicate, remove this conditional and move the player close to the the enemy, then move around/through the enemy erratically
			return;
		}

		this.MoveTowards (shortestPath [0]);
	}

	HashSet<Vector2> GetObstacles() {
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;

		HashSet<Vector2> obstacles = new HashSet<Vector2> ();

		foreach(GameObject anObject in allObjects) {
			// ignore self and the player
			if (anObject == this.gameObject || anObject == this.player) {
				continue;
			}

			// ignore objects that are not in the blocking layer

			// do math log to convert LayerMask.value to integer
			// http://answers.unity3d.com/questions/644123/layermask-can-someone-explain-why-is-256-8.html
			if (anObject.layer != Mathf.Log(this.layerMask.value, 2)) {
				continue;
			}

			obstacles.Add(EnemyMove.RoundVector(anObject.transform.position));
		}
		return obstacles;
	}

	private static Vector2 RoundVector(Vector2 inp) {
		return new Vector2 (Mathf.Round (inp.x), Mathf.Round (inp.y));
	}

	float DistanceBetweenObjects(GameObject firstObject, GameObject secondObject) {

		Vector2 firstPosition = firstObject.transform.position;
		Vector2 secondPosition = secondObject.transform.position;

		Vector2 dirToObject = firstPosition - secondPosition;

		RaycastHit2D hit = Physics2D.Raycast (firstPosition, dirToObject, Mathf.Infinity, this.layerMask.value);

		if (hit.collider == null) {
			throw new UnityException("RAY SHOULD ALWAYS HIT SOMETHING WTF");
		}

		if (hit.collider.gameObject == secondObject) {
			return Vector2.Distance (firstPosition, secondPosition);
		}
		return -1;
	}
}

class Graph {
	private Node startNode;

	public Graph(Vector2 start, Vector2 end, HashSet<Vector2>obstacles) {
		this.startNode = new Node (start, end, obstacles, 0, null);
	}

	public List<Vector2> ShortestPath(int levelLimit) {
		Node endNode = this.BreadthFirstSearch (levelLimit);
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
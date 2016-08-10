using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// player should be set to Default layer, and obstacleLayer should be set to BlockingLayer in inspector

public class Enemy : Creature {
	public float lootChance = 1f;
	public float aggroDistance = 5.0f;
	public float patrolRadius = 3.0f;
	public int touch_damage = 1;
	public float knockback = 200;
	public float active;
	public float maxActive = 2f;
	// reward when killing monster
	public int killScore = 100;

	//Private Entities
	private Animator anim;
	[SerializeField]
	protected GameObject player;
	private SpriteRenderer spriteRenderer;

	// set to BlockingLayer in the inspector plz
	public LayerMask obstacleLayer;

	private Vector2 anchorPosition;
	// how deep to do graph search
	private int GRAPH_SEARCH_LIMIT = 20;

	private float cameraSize;

	private Vector2 nextPatrolPosition;

	// caching variables
	// where we previously want to go (might have obstacles on the way)
	private Vector2 previousDestination;
	// sub destination on our route to the destination (no obstacles on the way)
	private Vector2 previousSubDestination;

	// states
	protected bool isAggroed = false;


	// Use this for initialization
	protected void Start () {
		base.Start();
		cameraSize = 2f * Camera.main.orthographicSize;
		anim = GetComponent<Animator>();
		
		spriteRenderer = GetComponent<SpriteRenderer>();
		// temporary comment out to fix bug
//		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

		player = GameObject.Find("Player");
		this.anchorPosition = this.transform.position;

		active = maxActive;
		// initialise nextPatrolPosition to current position, so update will know that it is time to assign a new position
		this.nextPatrolPosition = this.transform.position;
	}

	protected void Update () {
		if (active >= 0){
			active -= Time.deltaTime;
			spriteRenderer.color= new Color(255f, 255f, 255f, (maxActive - active) / maxActive);
			return;
		}
		base.Update();
		if (this.PlayerIsNear()) {
			this.isAggroed = true;
			this.MoveToPlayer ();
			return;
		}

		this.isAggroed = false;
		if (!this.AtNextPatrolPosition()) {
			this.MoveToLocation (this.nextPatrolPosition);
		} else {
			this.nextPatrolPosition = this.NewPatrolPoint ();
		}


	}

	public override void takeDamage(int dmg) {
		currHealth -= dmg;
		if (dmg > 0) {
			flash(flash_duration);
		}
		if (currHealth <= 0) {
			this.die();
		}
	}

	public override void die() {
		Destroy(gameObject);
		GameManager.instance.OnEnemyKilled (this.killScore);
		TestDropLoot();
	}

	public void TestDropLoot(){
		float roll = Random.value;
		Debug.Log(roll);
		if (roll <= lootChance){
			GameObject.FindGameObjectWithTag("WeaponPool").GetComponent<WeaponPool>().DropEnemyLoot(transform.position);	
		}
	}

	//Override to interact with player
	public virtual void contactPlayer(Collider2D other) {
		if(active > 0)
			return;
		other.GetComponent<Player>().takeDamage(touch_damage);
		Vector2 knockback_dir = other.transform.position - transform.position;
		other.GetComponent<Rigidbody2D>().AddForce(knockback * knockback_dir, ForceMode2D.Impulse);
	}

	//Basic Contact method
	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			contactPlayer(other);
		}
	}

	//If Player remains in contact, continue to trigger
	protected virtual void OnTriggerStay2D(Collider2D other) {
		OnTriggerEnter2D(other);
	}

	bool PlayerIsNear() {
		float distance = Vector2.Distance (this.transform.position, this.player.transform.position);
		return distance <= this.aggroDistance;
	}

	bool PlayerIsTooFar() {
		return Vector2.Distance (this.transform.position, this.player.transform.position) > cameraSize;
	}

	bool ClearPathToLocation(Vector2 position) {
		Vector2 dirToObject = position - (Vector2)this.transform.position;
		RaycastHit2D hit = Physics2D.Raycast (this.transform.position, dirToObject, Mathf.Infinity, this.obstacleLayer.value);
		// nothing hit means there's a clear path because the player is not on the blocking layer

		if (hit.collider == null) {
			return true;
		}

		// some blocking layer is hit
		Vector2 hitPoint = hit.transform.position;


		// we check the thing we hit, and if its further than the position we want to move to, there is a clear path to where we want to go
		return Vector2.Distance (this.transform.position, position) < Vector2.Distance (this.transform.position, hitPoint);
	}

	bool AtNextPatrolPosition() {
		return Vector2.Distance (this.transform.position, this.nextPatrolPosition) < 1.0f;
	}

	void MoveTowards(Vector2 position) {
		GetComponent<Rigidbody2D>().AddForce (this.moveScale * (position - (Vector2)this.transform.position).normalized);

		// cache where we are going for caching optimization in MoveToLocation
		this.previousSubDestination = RoundVector(position);
	}

	void MoveToPlayer() {
		this.MoveToLocation (this.player.transform.position);
	}

	void MoveToLocation(Vector2 position) {
		// caching optimization

		// if our desired destinationis the same as the previous frame and,
		// we are not at the desired sub destination yet,
		// just continue moving to the previous sub destination
		if (RoundVector(position) == this.previousDestination && RoundVector(this.transform.position) != this.previousSubDestination) {
			this.MoveTowards (this.previousSubDestination);
			return;
		}

		this.previousDestination = RoundVector(position);

		// if not thing in the way, just move to the player
		// else we have to do graph search
		if (this.ClearPathToLocation(position)) {
			this.MoveTowards(position);
			return;
		}

		Debug.Log (this.gameObject + " doing search");
		// get list of coordinates for blocking objects
		HashSet<Vector2> obstacles = this.GetObstacles();

		// do bfs and find a path towards the player
		Graph graph = new Graph (RoundVector(this.transform.position), RoundVector(position), obstacles);
		List<Vector2> shortestPath = graph.ShortestPath (GRAPH_SEARCH_LIMIT);

		if (shortestPath.Count < 1) {
			// workaround for the bug when the player is too close and somehow the ray cast in 
			// ClearPathToPlayer() thinks that there is no clear path, and we end up with a shortest path of length 0

			// to replicate, remove this conditional and move the player close to the the enemy, then move around/through the enemy erratically

			// also another workaround for where the player walks into a wall and no path can be found to the player
			return;
		}

		this.MoveTowards (shortestPath [0]);
	}

	HashSet<Vector2> GetObstacles() {
		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

		HashSet<Vector2> obstacles = new HashSet<Vector2> ();

		foreach(GameObject wall in walls) {
			obstacles.Add (wall.transform.position);
		}

		return obstacles;
	}

	private static Vector2 RoundVector(Vector2 inp) {
		return new Vector2 (Mathf.Round (inp.x), Mathf.Round (inp.y));
	}

	Vector2 NewPatrolPoint() {
		Vector2 newPoint;

		do {
			newPoint = Random.insideUnitCircle * this.patrolRadius + (Vector2)this.anchorPosition;
		} while (!this.ClearPathToLocation (newPoint));

		return newPoint;
	}
}


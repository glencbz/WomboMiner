using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float maxMoveForce = 1;
	public float moveScale = 50;
	public float maxVelocity = 10;
	public int maxHealth = 20;
	public int currHealth = 10;

	//Private Entities
	private Rigidbody2D rigidBody;
	private Collider2D collider2D;
	private SpriteRenderer spriteRenderer;
	private Animator anim;
	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		collider2D = this.GetComponent<Collider2D>();
		anim = GetComponent<Animator>();
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void takeDamage(int dmg) {
		currHealth -= dmg;
	}

	//Override to interact with player
	public virtual void contactPlayer(Collider2D other) {

	}

	//Basic Contact method
	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			contactPlayer(other);
		}
	}


}

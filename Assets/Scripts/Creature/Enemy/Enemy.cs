using UnityEngine;
using System.Collections;

public class Enemy : Creature {
	public float lootChance;
	public float aggroDistance = 5.0f;

	[HideInInspector]
	// anchor position where enemy will return to if player runs away
	public Vector2 anchorPosition;

	//Private Entities
	private Animator anim;

	// Use this for initialization
	protected void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		// temporary comment out to fix bug
//		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

		this.anchorPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void takeDamage(int dmg) {
		currHealth -= dmg;
		if (currHealth <= 0) {
			this.die();
		}
	}

	public override void die() {
		Debug.Log("Enemy killed");
		Destroy(gameObject);
	}

	public void testDropLoot(){
		if (Random.value <= lootChance)
			GameObject.FindGameObjectWithTag("LootPool").GetComponent<WeaponPool>().DropEnemyLoot(transform.position);
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

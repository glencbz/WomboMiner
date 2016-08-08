using UnityEngine;
using System.Collections;

public class Enemy : Creature {
	//Private Entities
	private Animator anim;
	public float lootChance;
	public float aggroDistance = 5.0f;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
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

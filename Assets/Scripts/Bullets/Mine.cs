using UnityEngine;
using System.Collections;

public class Mine : SimpleHitBox {
	public float armingTime = 2f;
	private bool armed = false;
	public Sprite unarmedSprite;
	public Sprite armedSprite;
	public GameObject explosion;
	
	// Update is called once per frame
	void Update () {
		armingTime -= Time.deltaTime;
		if (armingTime <= 0) {
			armed = true;
			gameObject.GetComponent<SpriteRenderer>().sprite = armedSprite;
			if (others.Count > 0) {
				others.RemoveWhere(i => !i);
				foreach(Collider2D c in others) {
					testCollision(c.tag);
				}
				
			}
		}
	}

	void asplode() {
		GameObject e = (GameObject) Instantiate(explosion, transform.position, Quaternion.identity);
		e.GetComponent<Explosion>().source = source;
		Destroy(gameObject);
	}

	void testCollision(string tag) {
		switch(tag) {
			case "Player":
			case "Enemy":
				if (tag != source) 
					asplode();
				break;
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);
		if (armed) {
			testCollision(other.tag);
			
		}
	}

}

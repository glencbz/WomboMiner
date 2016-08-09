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
				asplode();
			}
		}
	}

	void asplode() {
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);
		if (armed) {
			asplode();
		}
	}

}

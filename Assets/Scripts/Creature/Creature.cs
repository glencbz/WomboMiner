using UnityEngine;
using System.Collections;

public abstract class Creature : MonoBehaviour {
	public float maxMoveForce = 1;
	public float moveScale = 50;
	public float maxVelocity = 10;
	public int maxHealth = 20;
	public int currHealth = 10;
	protected Rigidbody2D rigidBody;
	protected SpriteRenderer spriteRenderer;

	// Damage Flash
	public Color c = Color.red;
	public float flash_duration = 0.3f;
	protected Material m;
	protected bool flash_flag = false;
	protected float timer;
	protected Color colorStart = Color.white;

	public abstract void takeDamage(int dmg);
	public abstract void die();

	protected void Start() {
		m = GetComponent<SpriteRenderer>().material;
	}
	protected void Update() {
		if (flash_flag == true) {
			float lerp = Mathf.PingPong(Time.time, flash_duration) / flash_duration;
        	m.color = Color.Lerp(colorStart, c, lerp);
		}
		timer -= Time.deltaTime;
		if (timer <= 0) {
			flash_flag = false;
			m.color = colorStart;
		}
	}
	public void flash(float f) {
		flash_flag = true;
		timer = f;
	}

}

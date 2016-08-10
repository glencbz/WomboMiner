using UnityEngine;
using System.Collections;

public abstract class Creature : MonoBehaviour {

	//Movement 
	public float maxMoveForce = 1;
	public float moveScale = 50;
	public float maxVelocity = 10;

	//Health
	public int maxHealth = 20;
	public int currHealth = 10;

	//Components
	protected Rigidbody2D rigidBody;
	protected SpriteRenderer spriteRenderer;
	protected Material m;

	// Damage Flash
	public Color damage_flash = Color.red;
	public float flash_duration = 0.3f;

	//Private Variables
	protected bool flash_flag = false;
	protected float timer;
	protected Color colorStart = Color.white;

	protected void Start() {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		rigidBody = this.GetComponent<Rigidbody2D>();
		m = GetComponent<SpriteRenderer>().material;
	}
	protected void Update() {
		if (flash_flag == true) {
			float lerp = Mathf.PingPong(Time.time, flash_duration) / flash_duration;
        	m.color = Color.Lerp(colorStart, damage_flash, lerp);
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

	public virtual void ApplyKnockback(Vector2 kb, float magnitude) {
		rigidBody.AddForce(kb * magnitude, ForceMode2D.Impulse);
	}
	public abstract void takeDamage(int dmg);
	public abstract void die();
}

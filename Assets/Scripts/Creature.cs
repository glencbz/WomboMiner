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

	public abstract void takeDamage(int dmg);
	public abstract void die();

}

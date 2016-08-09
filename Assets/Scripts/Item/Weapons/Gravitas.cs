using UnityEngine;
using System.Collections;

public class Gravitas : Weapon {

	public float magnitude = 1;
	public float range = 50;
	public int damage;
	//Produces a beam that sucks in enemies
	protected override void GenerateBullet(Vector3 mousePos) {
		Vector2 initialDirection = mousePos - transform.position;
		float angle = Vector2.Angle(Vector2.right, initialDirection);
		if (initialDirection.y < 0) {
			angle = 360 - angle;
		}

		//Small damage effect
		RaycastHit2D[] hits  = Physics2D.RaycastAll(transform.position, initialDirection, range);
		float StoppingDistance  = range;
		foreach(RaycastHit2D hit in hits) {
			if (hit.collider.tag == "Enemy") {
				hit.transform.GetComponent<Creature>().takeDamage(damage);
			}

			if (hit.collider.tag == "Wall") {
				StoppingDistance = hit.distance;
			}
		}
		//Gravity effect
		hits  = Physics2D.CircleCastAll(transform.position, size, initialDirection, StoppingDistance - size);
		foreach(RaycastHit2D hit in hits) {
			if (hit.collider.tag == "Enemy") {

				Vector2 dir = ((Vector2) hit.transform.position - hit.centroid).normalized;
				Vector2 projection = Vector2.Dot(dir, initialDirection.normalized) * initialDirection.normalized;
				Vector2 perp = projection - dir;
				hit.rigidbody.AddForce(perp * magnitude, ForceMode2D.Impulse);
			}
		}
	}
}

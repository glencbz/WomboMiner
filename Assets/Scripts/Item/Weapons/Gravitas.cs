using UnityEngine;
using System.Collections;

public class Gravitas : Weapon {

	public float magnitude = 1;
	public float range = 50;
	public int damage;
	private ParticleSystem[] ps;
	private LineRenderer lr;
	//Produces a beam that sucks in enemies

	protected void Start() {
		base.Start();
		ps = GetComponentsInChildren<ParticleSystem>();
		lr = GetComponent<LineRenderer>();
		
	}

	protected void Update() {
		base.Update();
		if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
			//Attempt to stop Particle
			foreach (ParticleSystem p in ps) {
				p.Stop();
				p.Clear();
			}
			lr.enabled = false;
		}
	}
	protected override void GenerateBullet(Vector3 mousePos) {
		Vector2 initialDirection = (mousePos - transform.position);
		float angle = Vector2.Angle(Vector2.right, initialDirection);
		if (initialDirection.y < 0) {
			angle = 360 - angle;
		}

		//Small damage effect
		RaycastHit2D[] hits  = Physics2D.RaycastAll(transform.position, initialDirection.normalized, range);
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
		hits  = Physics2D.CircleCastAll(transform.position, size, initialDirection.normalized, StoppingDistance - size);
		foreach(RaycastHit2D hit in hits) {
			if (hit.collider.tag == "Enemy") {

				Vector2 dir = ((Vector2) hit.transform.position - hit.centroid).normalized;
				Vector2 projection = Vector2.Dot(dir, initialDirection.normalized) * initialDirection.normalized;
				Vector2 perp = projection - dir;
				hit.rigidbody.AddForce(perp * magnitude, ForceMode2D.Impulse);
			}
		}

		//Particle effect

		foreach(ParticleSystem p in ps) {
			p.transform.localPosition = new Vector3(StoppingDistance / 2, 0, 0);
			var sh = p.shape;
			sh.radius = StoppingDistance / 2;
			var r = p.emission;
			r.rate = new ParticleSystem.MinMaxCurve(StoppingDistance * 5) ;
			if (!p.isPlaying) {p.Play();}
		}

		//Line effect
		lr.enabled = true;
		lr.SetPosition(0, new Vector3(gunpoint.x, gunpoint.y, 0));
		lr.SetPosition(1, new Vector3(StoppingDistance, 0, 0));
	}
}

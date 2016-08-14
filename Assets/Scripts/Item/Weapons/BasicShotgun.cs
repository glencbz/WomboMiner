using UnityEngine;
using System.Collections;

// Typical shotgun behaviour, fires bullets in a spread.
public class BasicShotgun : Weapon {

	//shotgun spread in degrees
	public float spread = 30f;
	public int numShot = 5;

	protected override void GenerateBullet(Vector3 mousePos){
		StartCoroutine("ShotgunSpread", mousePos);
		Animator a = GetComponentInChildren<Animator>();
		if (a) {a.SetTrigger("fire"); }
	}

	// Coroutine that causes the shotgun pellets to be 'bunched up' (typical shotgun expectation) rather than fly in a fan
	private IEnumerator ShotgunSpread(Vector3 mousePos){
		Vector3 initial = transform.position;
		for (int i = 0; i < numShot; i++){
			GenerateShot(RandomAngleVector(initial, mousePos));
			yield return new WaitForSeconds(0.0001f);
		}
	}

	protected virtual void GenerateShot(Vector3 mousePos){
		Bullet newBullet = (Bullet) Instantiate(bullet, transform.position + new Vector3(gunpoint.x, gunpoint.y, 0), transform.rotation);
		newBullet.source = "Player";
		newBullet.InitialFire(transform, mousePos);
	}

	// Returns a target vector that lies at a random angle from the line from player to mouse
	private Vector3 RandomAngleVector(Vector3 sourcePos, Vector3 mousePos){
		float angle = Random.value * spread - spread / 2;
		return sourcePos + (Quaternion.AngleAxis(angle, Vector3.forward) * (mousePos - sourcePos));
	}
}

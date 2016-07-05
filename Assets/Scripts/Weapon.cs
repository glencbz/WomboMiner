using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public Bullet bullet;
	public float cooldown;
	public float cooldownStatus;
	public bool noCooldown = true;

	void Start () {
		
	}

	void Update () {
		if (cooldownStatus > 0)
			cooldownStatus -= Time.deltaTime;
		else
			noCooldown = true;
	}

	public virtual void FireBullet(Vector2 direction){
		if (!noCooldown){
			noCooldown = false;
			GenerateBullet(direction);	
		}
	}

	protected virtual void GenerateBullet(Vector2 direction){
		Bullet newBullet = Instantiate(bullet);
		newBullet.InitialFire(direction);
	}
}

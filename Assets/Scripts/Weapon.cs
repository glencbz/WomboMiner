using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public Bullet bullet;
	public float cooldown;
	public float scaleSize = .2f;
	public Vector2 offset;
	private Quaternion rotation;

	public Sprite horizontalImage;
	public Sprite verticalImage;

	[HideInInspector]
	public float cooldownStatus = 0;

	private Player player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		transform.SetParent(player.transform, false);
		transform.position += (Vector3) offset;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
		transform.localScale *= scaleSize;
	}

	void Update () {
		cooldownStatus -= Time.deltaTime;
	}

	public virtual void FireBullet(Vector3 direction){
		if (cooldownStatus <= 0){
			cooldownStatus = cooldown;
			GenerateBullet(direction);	
		}
	}

	protected virtual void GenerateBullet(Vector3 mousePos){
		Bullet newBullet = Instantiate(bullet);
		newBullet.InitialFire(transform, mousePos);
	}
}

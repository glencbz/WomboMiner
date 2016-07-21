using UnityEngine;
using System.Collections;

public class Weapon : Item {

	public Bullet bullet;
	public float cooldown;
	public float scaleSize = .2f;
	public Vector2 offset;
	private Quaternion rotation;
	public Sprite itemImage; //Image to display when on ground
	public Sprite horizontalImage; //Image to display when in hand (face left/right)
	public Sprite verticalImage;//Image to display when in hand (face up/down)
	public float arrowhead = 90;
	public bool pointAtMouse = true;

	[HideInInspector]
	public float cooldownStatus = 0;

	private Player player;

	void Start () {
		base.Start();
		// player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		// transform.SetParent(player.transform, false);
		// transform.position += (Vector3) offset;
		// transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
		// transform.localScale *= scaleSize;
	}

	public override bool Pickup (Hand hand) {
		Debug.Log("ATTEMPT PICK UP");
		//Drop weapon in hand first
		bool dropped = hand.Drop();
		if (dropped) {
			//Attach weapon to hand
			transform.SetParent(hand.transform, false);
			transform.localPosition = Vector3.zero;
			//Disable Item Collider
			this.ToggleCollider(false);
		}
		return true;
	}


	public bool Drop() {
		//Position on floor
		transform.position = transform.parent.parent.position;
		//Detach from hand
		transform.parent = null;
		//Enable Item Collider
		this.ToggleCollider(true);
		return true;
	}

	void Update () {
		//Update cooldown
		if (cooldownStatus > 0) {
			cooldownStatus -= Time.deltaTime;
			if (cooldownStatus < 0) { cooldownStatus = 0; }
		}
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

	public virtual void updateSprite(float angle) {
		if (!pointAtMouse) {return;}
		transform.localEulerAngles = new Vector3(0, 0, angle - arrowhead);
	}
}

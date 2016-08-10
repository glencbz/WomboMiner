using UnityEngine;
using System.Collections;

public class Weapon : Item {

	public Bullet bullet;
	public float cooldown;

	//	[HideInInspector]
	public float cooldownStatus = 0;

	//Size of weapon and bullet
	public float size = 1f;
	public Vector2 gunpoint = Vector2.zero;
	public Sprite itemImage; //Image to display when on ground
	public Sprite horizontalImage; //Image to display when in hand (face left/right)
	public Sprite verticalImage;//Image to display when in hand (face up/down)
	public float arrowhead = 90;
	public bool pointAtMouse = true;//TODO: stop rotating if not pointAtMouse
	public bool isEnemySource = false;



	protected Player player;
	protected SpriteRenderer sr;

	protected void Start () {
		base.Start();
		sr = GetComponentInChildren<SpriteRenderer>();	
	
		if (isEnemySource) {
			sr.enabled = false;
		}
	}
	
	//Pickup method for Player to call when picking up item. Drops weapon in hand first before picking up.
	public override bool Pickup (Hand hand) {
		//Equip in hand
		bool equipped = hand.Equip(this);
		if (equipped) {
			//Disable Item Collider
			this.ToggleCollider(false);
			sr.sortingLayerName = "Player";
			return true;
		} else {
			return false;
		}
		
	}


	public bool Drop() {
		//Position on floor
		transform.position = transform.parent.parent.position;
		//Detach from hand
		transform.parent = null;
		//Enable Item Collider
		this.ToggleCollider(true);
		sr.sprite = itemImage;
		sr.sortingLayerName = "Items";
		//reset rotation
		transform.rotation = Quaternion.identity;
		return true;
	}

	protected virtual void Update () {
		//Update cooldown
		if (cooldownStatus > 0) {
			cooldownStatus -= Time.deltaTime;
			if (cooldownStatus < 0) { cooldownStatus = 0; }
		}
	}
	//Method for firing a bullet.
	//CAN YOU FIRE?
	public virtual void FireBullet(Vector3 direction){
		if (cooldownStatus <= 0){

			cooldownStatus = cooldown;
			GenerateBullet(direction);	
		}
	}

	//HOW DO YOU FIRE?
	protected virtual void GenerateBullet(Vector3 mousePos){
		Bullet newBullet = (Bullet) Instantiate(bullet, transform.position + new Vector3(gunpoint.x, gunpoint.y, 0), transform.rotation);

		if (isEnemySource) {
			newBullet.source = "Enemy";
		} else {
			newBullet.source = "Player";			
		}


		newBullet.InitialFire(transform, mousePos);
	}

	public virtual void updateSprite(float angle) {
		transform.localEulerAngles = new Vector3(0, 0, angle - arrowhead);
		
		if (angle > 45 && angle <= 135) {//UP
			sr.sprite = verticalImage;
			sr.flipY = false;
			//sr.sortingOrder = 4;
		} else if (angle > 135 && angle <= 225) {//LEFT
			sr.sprite = horizontalImage;
			sr.flipY = true;
			//sr.sortingOrder = 6;
		} else if (angle > 225 && angle <= 315) {//DOWN
			sr.sprite = verticalImage;
			sr.flipY = false;
			//sr.sortingOrder = 7;
		} else {//RIGHT
			sr.sprite = horizontalImage;
			sr.flipY = false;
			//sr.sortingOrder = 4;
		}
		sr.sortingOrder = transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
	}
}

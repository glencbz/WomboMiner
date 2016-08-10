using UnityEngine;
using System.Collections;

// Reduces cooldown based on how much HP the player has
// Will only work with player at the moment
public class BeserkerGun : Weapon {

	public float modifier = 2f;
	public float minCooldown = 0.2f;
	// need an unmodified cooldown status to maintain correctness across time
	private float rawCooldownStatus = 0;
	private float rawCooldown;

	private Player player;

	void Start(){
		base.Start();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		rawCooldown = cooldown;
	}

	protected override void Update(){
		if (rawCooldownStatus > 0) {
			rawCooldownStatus -= Time.deltaTime ;
			if (rawCooldownStatus < 0) { rawCooldownStatus = 0; }
		}
		cooldownStatus = rawCooldownStatus - CdReduction();
		cooldown = rawCooldown - CdReduction();
	}

	protected override void GenerateBullet(Vector3 mousePos){
		base.GenerateBullet(mousePos);
		Animator a = GetComponentInChildren<Animator>();
		if (a) {a.SetTrigger("fire"); }
	}

	public override void FireBullet(Vector3 direction){
		if (cooldownStatus <= 0){
			rawCooldownStatus = rawCooldown;
			GenerateBullet(direction);	
		}
	}

	private float CdReduction(){
		return (player.maxHealth - player.currHealth) * modifier;
	}
}

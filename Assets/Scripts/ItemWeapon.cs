using UnityEngine;
using System.Collections;

public class ItemWeapon : Item {

	public Weapon weapon;

	public override void Pickup () {
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		player.heldWeapons[player.weaponToReplace] = weapon;

		gameObject.SetActive(false);
	}
}

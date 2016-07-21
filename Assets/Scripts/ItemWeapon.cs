using UnityEngine;
using System.Collections;

public class ItemWeapon : Item {

	public Weapon weapon;

	public override bool Pickup (Hand hand) {
		Weapon weapon = hand.transform.GetChild(0).gameObject.GetComponent<Weapon>();
		//	weapon.Drop();
		//TODO: drop player item in hand first
		//player.heldWeapons[hand] = Instantiate(weapon);
		gameObject.SetActive(false);
		return true;
	}
}

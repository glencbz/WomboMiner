using UnityEngine;
using System.Collections;

public class ItemWeapon : Item {

	public Weapon weapon;

	public override bool Pickup (int hand) {
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		//TODO: drop player item in hand first
		player.heldWeapons[hand] = Instantiate(weapon);
		gameObject.SetActive(false);
		return true;
	}
}

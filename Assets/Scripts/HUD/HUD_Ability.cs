using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Ability : MonoBehaviour {
	public Player player;
	public Image abilityA, abilityB;
	public Image cooldownA, cooldownB;
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		abilityA = transform.GetChild(0).GetComponent<Image>();
		abilityB = transform.GetChild(1).GetComponent<Image>();
		cooldownA = abilityA.transform.GetChild(0).GetComponent<Image>();
		cooldownB = abilityB.transform.GetChild(0).GetComponent<Image>();
	}

	void Update () {
		if (player.hands.left.checkEmpty()) {
			abilityA.sprite = null;
		} else {
			abilityA.sprite = player.hands.left.weapon.itemImage;
			cooldownA.fillAmount = player.hands.left.weapon.cooldownStatus / player.hands.left.weapon.cooldown;
		}

		if (player.hands.right.checkEmpty()) {
			abilityB.sprite = null;
		} else {
			abilityB.sprite = player.hands.right.weapon.itemImage;
			cooldownB.fillAmount = player.hands.right.weapon.cooldownStatus / player.hands.right.weapon.cooldown;
		}
	}
}

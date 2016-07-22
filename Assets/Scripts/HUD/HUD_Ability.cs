using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Ability : MonoBehaviour {

	public Player player;
	public Image abilityA, abilityB;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		abilityA = transform.GetChild(0).GetComponent<Image>();
		abilityB = transform.GetChild(1).GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.hands.left.checkEmpty()) {
			abilityA.sprite = null;
		} else {
			abilityA.sprite = player.hands.left.weapon.itemImage;
		}

		if (player.hands.right.checkEmpty()) {
			abilityB.sprite = null;
		} else {
			abilityB.sprite = player.hands.right.weapon.itemImage;
		}
	}
}

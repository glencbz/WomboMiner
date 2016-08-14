using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD_HP : MonoBehaviour {
	/*
	HUD HP module. Reads values from Player object and displays it on top right corner of screen.
	Refreshes on every frame.
	*/
	public int HP_per_heart = 2;
	public int maxHP = 10;
	public int hp = 10;
	private int MAX_CONTAINERS = 10;

	GameObject player;
	private int current_containers = 0;
	public GameObject heartContainer;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		maxHP = player.GetComponent<Player>().maxHealth;
		hp = player.GetComponent<Player>().currHealth;
		initContainers();
	}

	public void FixedUpdate() {
		//Update HUD data with Player data
		updateData();
		//Clamp to maxHP
		if (hp > maxHP) {
			hp = maxHP;
		}
		//Display the new HP
		float fillPoint = hp;
		for (int i=0; i<current_containers;i++) {
			fillPoint -= HP_per_heart;
			if (fillPoint >= 0) {
				transform.GetChild(i).GetComponent<heartFillScript>().fill(1);
			} else if (fillPoint <= -HP_per_heart) {
				transform.GetChild(i).GetComponent<heartFillScript>().fill(0.0f);
			} else {
				transform.GetChild(i).GetComponent<heartFillScript>().fill(1 + (fillPoint / HP_per_heart));
			}
		}
	}

	//Update HUD values with Player's most recent values
	void updateData() {
		maxHP = player.GetComponent<Player>().maxHealth;
		hp = player.GetComponent<Player>().currHealth;
		if (maxHP != current_containers * HP_per_heart) {
			initContainers();
		}
	}

	public void initContainers() {
		current_containers = maxHP / HP_per_heart;
		for (int i = 0; i < MAX_CONTAINERS; i++) {
			if (i < current_containers) {transform.GetChild(i).gameObject.SetActive(true); }
			else {transform.GetChild(i).gameObject.SetActive(false); }
		}

	}

}

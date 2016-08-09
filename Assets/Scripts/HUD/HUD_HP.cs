using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD_HP : MonoBehaviour {

	public int HP_per_heart = 2;
	public int maxHP = 10;
	public int hp = 10;

	GameObject player;
	private List<GameObject> hearts;
	public GameObject heartContainer;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		maxHP = player.GetComponent<Player>().maxHealth;
		hp = player.GetComponent<Player>().currHealth;

		//Init hearts equivalent to maxHP 
		hearts = new List<GameObject>();
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
		for (int i=0; i<hearts.Count;i++) {
			fillPoint -= HP_per_heart;
			if (fillPoint >= 0) {
				hearts[i].GetComponent<heartFillScript>().fill(1);
			} else if (fillPoint <= -HP_per_heart) {
				hearts[i].GetComponent<heartFillScript>().fill(0.0f);
			} else {
				hearts[i].GetComponent<heartFillScript>().fill(1 + (fillPoint / HP_per_heart));
			}
		}
	}

	void updateData() {
		maxHP = player.GetComponent<Player>().maxHealth;
		hp = player.GetComponent<Player>().currHealth;
		if (maxHP != hearts.Count * HP_per_heart) {
			initContainers();
		}
	}

	public void initContainers() {
		//Clear all existing hearts first
		foreach(GameObject heart in hearts) {
				Destroy(heart);
		}

		float offset = this.GetComponent<RectTransform>().rect.width / 12;
		for (int i=0;i<(maxHP / HP_per_heart);i++) {
			GameObject heart = Instantiate(heartContainer);
			hearts.Add(heart);
			heart.transform.SetParent(this.gameObject.transform);
			//heart.GetComponent<RectTransform>().localPosition = new Vector3(30*i,0,0);
			//Set heart position
			RectTransform rt = heart.GetComponent<RectTransform>();
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, offset*(i+1),rt.rect.width);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, this.GetComponent<RectTransform>().rect.height / 4,rt.rect.height);
		}

	}

}

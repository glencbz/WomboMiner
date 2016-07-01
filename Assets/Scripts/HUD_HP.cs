using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD_HP : MonoBehaviour {

	public int maxHP;
	public int hp;

	private List<GameObject> hearts;
	public GameObject heartContainer;
	// Use this for initialization
	void Start () {
		//Init hearts equivalent to maxHP (1 heart = 2 HP)
		hearts = new List<GameObject>();
		for (int i=0;i<(maxHP / 2);i++) {
			GameObject heart = Instantiate(heartContainer);
			hearts.Add(heart);
			heart.transform.SetParent(this.gameObject.transform);
			//heart.GetComponent<RectTransform>().localPosition = new Vector3(30*i,0,0);
			RectTransform rt = heart.GetComponent<RectTransform>();
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 15+30*i,rt.rect.width);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, this.GetComponent<RectTransform>().rect.height / 4,rt.rect.height);
		}

		setHP(hp);
	}
	
	public void setHP(int newHP) {
		//Sets HP value to newHP and changes the HUD display
		//Clamp to maxHP
		if (newHP > maxHP) {
			hp = maxHP;
		} else {
			hp = newHP;
		}
		
		//Display the new HP
		for (int i=0; i<hearts.Count;i++) {
			if ((i+1)*2 <= hp) {
				hearts[i].GetComponent<heartFillScript>().fill(1);
			} else if ((i+1)*2-1 == hp) {
				hearts[i].GetComponent<heartFillScript>().fill(0.5f);
			} else {
				hearts[i].GetComponent<heartFillScript>().fill(0.0f);
			}
		}

	}

}

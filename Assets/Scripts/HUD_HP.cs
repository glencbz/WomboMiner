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
		//Init hearts equivalent to maxHP
		hearts = new List<GameObject>();
		for (int i=0;i<maxHP;i++) {
			GameObject heart = Instantiate(heartContainer);
			hearts.Add(heart);
			heart.transform.SetParent(this.gameObject.transform);
			//heart.GetComponent<RectTransform>().localPosition = new Vector3(30*i,0,0);
			RectTransform rt = heart.GetComponent<RectTransform>();
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 15+30*i,rt.rect.width);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, this.GetComponent<RectTransform>().rect.height / 4,rt.rect.height);
		}

		hp = maxHP;
	}
	
	public void setHP(int newHP) {
		hp = newHP;
	}

	public void takeDamage(int dmg) {

	}
}

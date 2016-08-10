using UnityEngine;
using System.Collections;

public class EnemyActiveTrigger : MonoBehaviour {
	GameObject player;

	void Start() {
		player = GameObject.Find("Player");
	}
	void Update() {
		this.transform.position = this.player.transform.position;
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag != "Enemy") {
			return;
		}
		other.gameObject.GetComponent<Enemy> ().enabled = true;
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag != "Enemy") {
			return;
		}
		other.gameObject.GetComponent<Enemy> ().enabled = false;
	}
}

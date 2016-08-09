using UnityEngine;
using System.Collections;

public class EnemyActiveTrigger : MonoBehaviour {
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

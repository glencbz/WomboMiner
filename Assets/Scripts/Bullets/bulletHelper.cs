using UnityEngine;
using System.Collections;

public class bulletHelper : MonoBehaviour {

	public void hit() {
		transform.parent.GetComponentInChildren<Bullet>().hitScan();
	}
}

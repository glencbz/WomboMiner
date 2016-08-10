using UnityEngine;
using System.Collections;

public class DeathScreen : MonoBehaviour {

	public void ExitToMainMenu() {
		GameManager.instance.GoBackToMainMenu ();
	}
}

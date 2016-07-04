using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class heartFillScript : MonoBehaviour {

	public Image heartFill;

	public void fill(float f) {
		heartFill.fillAmount = f;
	}
}

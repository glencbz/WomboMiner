using UnityEngine;
using System.Collections;

public class takeDamageFlash : MonoBehaviour {

	public Color c;
	public float duration = 0.2f;
	private Material m;
	private bool flash_flag = false;
	private float timer;
	private Color colorStart = Color.white;
	// Use this for initialization
	void Start () {
		//m = GetComponent<Material>();
		m = GetComponent<SpriteRenderer>().material;
	}
	
	//Flash for f duration
	public void flash(float f) {
		flash_flag = true;
		timer = f;
	}
	// Update is called once per frame
	void Update () {
		if (flash_flag == true) {
			float lerp = Mathf.PingPong(Time.time, duration) / duration;
        	m.color = Color.Lerp(colorStart, c, lerp);
		}
		timer -= Time.deltaTime;
		if (timer <= 0) {
			flash_flag = false;
			m.color = colorStart;
		}

	}
}

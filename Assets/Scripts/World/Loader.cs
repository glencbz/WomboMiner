using UnityEngine;
using System.Collections;

	public class Loader : MonoBehaviour 
	{
		public GameObject gameManager;			//GameManager prefab to instantiate.
		public GameObject soundManager;			//SoundManager prefab to instantiate.
		
		
		void Awake ()
		{
			//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
			if (GameManager.instance == null) {
				//Attempt to find existing GameManager
				GameObject g = GameObject.Find("GameManager");
				if (g) {
					GameManager.instance = g.GetComponent<GameManager>();
				} else {
					Debug.Log("Creating new GameManager");
					//Instantiate gameManager prefab
					Instantiate(gameManager);
				}

			}

			
			//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
			if (SoundManager.instance == null) {
				Debug.Log("Creating new SoundManager");
				//Instantiate SoundManager prefab
				Instantiate(soundManager);
			}
				

		}
	}

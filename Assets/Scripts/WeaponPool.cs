using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Script that handles the dropping of weapons. The function DropEnemyLoot is called by an enemy when an enemy should drop loot
public class WeaponPool : MonoBehaviour {

	//probabilities of a weapon from a given tier dropping, stronger weapons should drop less frequently
	public float[] enemyTierProbs = {.571428571f, .285714286f, .142857143f};
	private List<Weapon[]> tieredWeapons;

	// Import paths for each weapon tier
	public string[] weaponPaths = {"Tier1", "Tier2", "Tier3"};
	
	void Start () {
		tieredWeapons = new List<Weapon[]>();
		foreach (string path in weaponPaths){
			var loadedObjects = Resources.LoadAll(path, typeof(GameObject)).Cast<GameObject>().ToArray();
			Weapon[] weaponTier = new Weapon[loadedObjects.Length];
			foreach (var thing in loadedObjects)
				Debug.Log(thing);
			for (int i = 0; i < loadedObjects.Length; i++)
				weaponTier[i] = loadedObjects[i].GetComponent<Weapon>();
			tieredWeapons.Add(weaponTier);	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Calculates a loot drop for a killed enemy
	public void DropEnemyLoot(Vector3 position){
		int tier = CalculateTier(enemyTierProbs);
		// Uniformly chooses an item in the tier and instantaiates it at a given position.
		int roll = Random.Range(0, tieredWeapons[tier].Length);
		Weapon droppedWeapon = tieredWeapons[tier][roll];
		Instantiate(droppedWeapon.gameObject, position, Quaternion.identity);
	}

	// Finds the tier of weapon to be dropped according to the probabilities described in enemyTierProbs
	public int CalculateTier(float[] tierArray){
		float roll = Random.value;
		float acc = 0;
		for (int i = 0; i < tierArray.Length; i++){
			acc += tierArray[i];
			if (roll <= acc)
				return i;
		}
		// Theoretically will not be thrown unless the probabilities do not add up to 1
		throw new UnityException("Damn son your item drop exceeds math");
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponPool : MonoBehaviour {

	public float[] enemyTierProbs = {.571428571f, .285714286f, .142857143f};
	private List<Weapon[]> tieredWeapons;


	// See https://docs.unity3d.com/ScriptReference/Resources.html on how to create resources folders
	public string[] weaponPaths = {"Tier1", "Tier2", "Tier3"};
	
	void Start () {
		foreach (string path in weaponPaths)
			tieredWeapons.Add((Weapon[]) Resources.LoadAll(path));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Calculates a loot drop for a killed enemy
	public void DropEnemyLoot(Vector3 position){
		int tier = CalculateTier(enemyTierProbs);
		int roll = Random.Range(0, tieredWeapons[tier].Length);
		Weapon droppedWeapon = tieredWeapons[tier][roll];
		Instantiate(droppedWeapon.gameObject, position, Quaternion.identity);
	}
		
	public int CalculateTier(float[] tierArray){
		float roll = Random.value;
		float acc = 0;
		for (int i = 0; i < tierArray.Length; i++){
			acc += tierArray[i];
			if (roll <= acc)
				return i;
		}
		throw new UnityException("Damn son your item drop exceeds math");
	}
}

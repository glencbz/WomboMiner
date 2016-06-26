using UnityEngine;
using System.Collections;

using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public static class SaveLoad {
	public static Score saveFile;
	public static void Save() {
		BinaryFormatter bf = new BinaryFormatter();
    	FileStream file = File.Create (Application.persistentDataPath + "/sampleHighScore.gd");
    	bf.Serialize(file, Score.current);
    	file.Close();
	}

	public static bool Load() {
		if(File.Exists(Application.persistentDataPath + "/sampleHighScore.gd")) {
			Debug.Log("file found");
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/sampleHighScore.gd", FileMode.Open);
			saveFile = (Score) bf.Deserialize(file);
			file.Close();
			return true;
		} else{
			Debug.Log("No file found");
			return false;
		}
	}
}

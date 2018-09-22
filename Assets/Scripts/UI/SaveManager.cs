using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour 
{
	public static SaveManager instance;

	public int fileID;

	[Serializable]
	public class SaveFile 
	{
		public string name;
		public string scene;
		public Vector3 pos;
		public Vector3 rot;
		public float health;
		public float maxHealth;
	}

	void Awake ()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}	
		else
		{
			Destroy(gameObject);
		}
	}

	public static void Create (int fileID, string name)
	{
		SaveFile saveFile = new SaveFile();
		saveFile.scene = "Overworld";
		saveFile.pos = new Vector3 (23, 0, 12);
		saveFile.name = name;
		saveFile.health = 3;
		saveFile.maxHealth = 3;
		
		string fileName = "SaveFile" + fileID + ".json";
		string path = Path.Combine(Application.persistentDataPath, fileName);
		string json = JsonUtility.ToJson(saveFile);

		File.WriteAllText(path,json);
	}

	public static void Save ()
	{
		SaveFile saveFile = Read(instance.fileID);
		saveFile.scene = SceneManager.GetActiveScene().name;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		saveFile.pos = player.transform.position;
		saveFile.rot = player.transform.localEulerAngles;
		HealthController health = player.GetComponent<HealthController>();
		saveFile.health = health.health;
		saveFile.maxHealth = health.maxHealth;

		string fileName = "SaveFile" + instance.fileID + ".json";
		string path = Path.Combine(Application.persistentDataPath, fileName);
		string json = JsonUtility.ToJson(saveFile);

		File.WriteAllText(path,json);
	}

	public static SaveFile Read (int fileID)
	{
		string fileName = "SaveFile" + fileID + ".json";
		string path = Path.Combine(Application.persistentDataPath, fileName);
		if (!File.Exists(path)) 
			return null;
		string json = File.ReadAllText(path);
		return JsonUtility.FromJson<SaveFile>(json);
	}

	public static void Load (int fileID)
	{
		instance.fileID = fileID;
		SaveFile saveFile = Read (fileID);
		instance.StartCoroutine(instance.LoadFile(saveFile));
	}

	IEnumerator LoadFile (SaveFile saveFile)
	{
		yield return SceneManager.LoadSceneAsync(saveFile.scene);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = saveFile.pos;
		player.transform.localEulerAngles = saveFile.rot;
		HealthController health = player.GetComponent<HealthController>();
		health.health = saveFile.health;
		health.maxHealth = saveFile.maxHealth;
	}

	public static void Delete (int fileID)
	{
		string fileName = "SaveFile" + fileID + ".json";
		string path = Path.Combine(Application.persistentDataPath, fileName);
		if (File.Exists(path)) 
			File.Delete(path);
	}
}

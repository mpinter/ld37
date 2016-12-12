using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	string currentSceneName;

	// Use this for initialization
	void Start () {
		currentSceneName = SceneManager.GetActiveScene().name;
		if (!PlayerPrefs.HasKey("MaxLevel") && currentSceneName.Equals("MainMenu")) {
			PlayerPrefs.SetInt("MaxLevel", 0);
		}
		PlayerPrefs.Save();
	}

	public void LoadLevel(string sceneName) {
		Debug.Log("button: " + sceneName + " clicked");
		if (sceneName.Contains("Level")){
			int levelNum = Int32.Parse(sceneName.Substring(5));
			PlayerPrefs.SetInt("CurrentLevel", levelNum);
			if (levelNum > PlayerPrefs.GetInt("MaxLevel")) {
				PlayerPrefs.SetInt("MaxLevel", levelNum);
			}
			SceneManager.LoadScene("Level", LoadSceneMode.Single);
			return;
		}
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
	
	public void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update () {
		
	}
}

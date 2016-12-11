using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
	
	}

	public void LoadLevel(string sceneName) {
		Debug.Log("button: " + sceneName + " clicked");
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

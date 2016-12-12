﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorScene : MonoBehaviour {

	public GameObject buttonPrefab;
	private GameObject levelsPanel;

	// Use this for initialization
	void Start () {
		levelsPanel = GameObject.FindWithTag("LevelsPanel");
		for (int i = 0; i < PlayerPrefs.GetInt("MaxLevel")+1; i++) {
			var b = Instantiate(buttonPrefab) as GameObject;
			b.transform.SetParent(levelsPanel.transform, false);
			b.name = i.ToString();
			b.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
			b.GetComponent<Button>().onClick.AddListener(delegate{StartLevel(i);});
		}
	}
	
	void StartLevel(int i) {
		GetComponent<LevelLoader>().LoadLevel("Level"+i);
	}
}

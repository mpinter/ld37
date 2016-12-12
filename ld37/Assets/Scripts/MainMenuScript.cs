using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	public GameObject levelsPanel;
	public GameObject menuPanel;

	// Use this for initialization
	void Start () {
		menuPanel.SetActive(true);
		levelsPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void clickStartButton() {
		levelsPanel.SetActive(true);
		menuPanel.SetActive(false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flyyoufools;
using UniRx;
using System;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour {

	public int sanity = 10;
	public int sanityMax = 10;
	public GameObject sanityBar;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<InputScript>().Spacebar
		.Where(v => {
			return v != false;
		})
		//.Throttle(TimeSpan.FromMilliseconds(500))
		.Subscribe( val => {
			enemyTurn();
		})
		.AddTo(this);
		sanityBar.GetComponentInChildren<Image>().fillMethod=Image.FillMethod.Vertical;
        sanityBar.GetComponentInChildren<Image>().type=Image.Type.Filled;
		sanityBar.GetComponentInChildren<Image>().enabled = true;
	}

	void enemyTurn() {
		sanity = Math.Min(10, sanity+4);
		this.gameObject.GetComponent<MapScript>().enemyTurn();
		//TODO all of the other stuff
	}

	public void movePlayer() {
		sanity -= 1;
	}

	public void movePossesed() {
		sanity -= 2;
	}

	public void moveUnpossesed() {
		sanity -= 1;
	}

	void Update() {
		Debug.Log(sanityBar.GetComponentInChildren<Image>().fillAmount);
		sanityBar.GetComponentInChildren<Image>().fillAmount=(float)sanity/(float)sanityMax;
		// heh ,this is lame
		if (sanity < 1) {
			gameOver();
		}
	}

	public void gameOver() {
		Debug.Log("To be called");
	}
}

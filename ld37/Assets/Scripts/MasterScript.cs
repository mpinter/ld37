using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flyyoufools;
using UniRx;
using System;

public class MasterScript : MonoBehaviour {

	public int sanity = 10;

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
	}

	void enemyTurn() {
		this.gameObject.GetComponent<MapScript>().enemyTurn();
		//TODO all of the other stuff
	}

	public void gameOver() {
		Debug.Log("To be called");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

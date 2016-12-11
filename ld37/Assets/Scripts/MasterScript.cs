using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using flyyoufools;
using UniRx;
using System;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour {

	public int sanity = 10;
	public int sanityMax = 10;
	public GameObject sanityBar;

	public bool blockInput = false;
	
	private bool fadeIn = false;
	private bool faded = false;
	private bool fadeOut = false;
	private Image fader;
	private float fadeTimeLeft;
	private float fadeTimeStart;

	// Use this for initialization
	void Start () {
		var inputScript = this.gameObject.GetComponent<InputScript>(); 
		inputScript.Spacebar
		.Where(v => {
			return v != false;
		})
		//.Throttle(TimeSpan.FromMilliseconds(500))
		.Subscribe( val => {
			enemyTurn();
		})
		.AddTo(this);

		inputScript.Escape
		.Where(v => {
			return v != false;
		})
		//.Throttle(TimeSpan.FromMilliseconds(500))
		.Subscribe( val => {
			//Debug.Log("should go back");
			SceneManager.LoadScene("Selector", LoadSceneMode.Single);
			//enemyTurn();
		})
		.AddTo(this);
		
		sanityBar.GetComponentInChildren<Image>().fillMethod=Image.FillMethod.Vertical;
        sanityBar.GetComponentInChildren<Image>().type=Image.Type.Filled;
		sanityBar.GetComponentInChildren<Image>().enabled = true;

		fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Image>();
	}

	void startFade() {
		blockInput = true;
		fadeIn = true;
		fadeTimeStart = 0.2f;
		fadeTimeLeft = 0.2f;
	}

	void enemyTurn() {
		// fadeout
		// blockInput = true;
		// fadeIn = true;
		// Debug.Log("Should fadeout");
		// var fadeImage = GameObject.FindGameObjectWithTag("Fader").GetComponent<Image>();
		// var FADE_TIME = 0.2f;
		// fadeImage.CrossFadeAlpha(5.0f, FADE_TIME, false);
		// fadeTimeLeft = FADE_TIME;
		// fadeImage.color = new Color(0f, 0f, 0f, 0.2f);
		
		startFade();

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
		//Debug.Log(sanityBar.GetComponentInChildren<Image>().fillAmount);
		sanityBar.GetComponentInChildren<Image>().fillAmount=(float)sanity/(float)sanityMax;
		// heh ,this is lame
		if (sanity < 1) {
			gameOver();
		}


		if (faded) {
			fadeTimeLeft -= Time.deltaTime;
			if (fadeTimeLeft < 0f) {
				faded = false;
				fadeOut = true;
				
				fadeTimeStart = 2f;
				fadeTimeLeft = 2f;
			}
		}
		if (fadeOut) {
			Debug.Log("fadeOut " + fadeTimeLeft);
			fadeTimeLeft -= Time.deltaTime;
			if (fadeTimeLeft < 0f) {
				fader.color = new Color(0, 0, 0, 0);
				fadeOut = false;
				blockInput = false;
			} else {
				// fade to black
				fader.color = new Color(0, 0, 0, fadeTimeLeft / fadeTimeStart);
			}
			
		}
		if (fadeIn) {
			Debug.Log("fadeIn");
			fadeTimeLeft -= Time.deltaTime;
			if (fadeTimeLeft < 0f) {
				fader.color = new Color(0, 0, 0, 1f);
				fadeIn = false;
				faded = true;

				fadeTimeStart = 2f;
				fadeTimeLeft = 2f;
			} else {
				// fade to transparent
				fader.color = new Color(0, 0, 0, 1f - fadeTimeLeft / fadeTimeStart);	
			}
		}
		

	}

	public void gameOver() {
		Debug.Log("To be called");
	}
}

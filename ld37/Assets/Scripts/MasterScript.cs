using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using flyyoufools;
using UniRx;
using System;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour {

	public float sanity = 10.0f;
	public float sanityMax = 10.0f;
	public float sanityTarget = 10.0f;
	public GameObject sanityBar;
	private GameObject gameOverPanel;
	private GameObject youWinPanel;
	private GameObject newRoundPanel;

	public bool blockInput = false;
	
	private bool fadeIn = false;
	private bool totalFadeIn = false;
	private bool faded = false;
	private bool fadeOut = false;
	private bool gameOverBool = false;
	private Image fader;
	private float fadeTimeLeft;
	private float fadeTimeStart;
	private List<EntityType> entitiesToSpawn = new List<EntityType>();

	public int currentRound = 1;
	public int numRounds = 4;

	// Use this for initialization
	void Start () {
		gameOverPanel = GameObject.FindGameObjectWithTag("GameOverPanel");
		youWinPanel = GameObject.FindGameObjectWithTag("YouWinPanel");
		newRoundPanel = GameObject.FindGameObjectWithTag("NewRoundPanel");
		gameOverPanel.SetActive(false);
		youWinPanel.SetActive(false);
		newRoundPanel.SetActive(false);
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

	void newRoundFade() {
		blockInput = true;
		fadeIn = true;
		fadeTimeStart = 0.2f;
		fadeTimeLeft = 0.2f;
		newRoundPanel.SetActive(true);
		//newRoundPanel.gameObject.GetComponentsInChildren<Text>()[0].text = "Round " + currentRound + " / " + numRounds;  
	}

	void gameOverFade() {
		blockInput = true;
		totalFadeIn = true;
		fadeTimeStart = 0.2f;
		fadeTimeLeft = 0.2f;
		gameOverPanel.SetActive(true);
	}

	void youWinFade() {
		blockInput = true;
		totalFadeIn = true;
		fadeTimeStart = 0.2f;
		fadeTimeLeft = 0.2f;
		youWinPanel.SetActive(true);
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
		this.gameObject.GetComponent<MapScript>().enemyTurn();		
		EntityType[] copyToSpawn = new EntityType[entitiesToSpawn.Count]; 
		entitiesToSpawn.CopyTo(copyToSpawn);
		entitiesToSpawn.Clear();
		// spawn those exorcised in previous round
		for (int i=0; i<copyToSpawn.Length; i++) {
			GameObject[] unpossesed = GameObject.FindGameObjectsWithTag("Wall");
			foreach (GameObject obj in unpossesed) {
				if (obj.GetComponent<Entity>().entityType == copyToSpawn[i]) {
					var entity = obj.GetComponent<Entity>();
					var animator = entity.GetComponent<Animator>();
					entity.tag = "Enemy";
					switch(entity.entityType) {
						case EntityType.ChargingEnemy:
							animator.SetInteger("Direction", entity.chargingDirection);
							break;
						case EntityType.ChasingEnemy:
							break;
						case EntityType.RookEnemy:
							animator.SetInteger("Direction", entity.rookState ? 2 : 1);
							break;
					}
					Debug.Log("Spawn");
					break;
				}
			}
		}

		currentRound +=1;
		newRoundPanel.GetComponentInChildren<Text>().text = "Round " + currentRound + "/" + numRounds;
		if (gameOverBool) {
			gameOverFade();
		} else if (currentRound > numRounds) {
			youWinFade();
		} else {
			sanity = Math.Min(10, sanity+4);			
			newRoundFade();
		}		
	}

	public void addToSpawnQueue(EntityType et) {
		entitiesToSpawn.Add(et);
	}

	public void movePlayer() {
		sanityTarget -= 1;
	}

	public void movePossesed() {
		sanityTarget -= 2;
	}

	public void moveUnpossesed() {
		sanityTarget -= 1;
	}

	void Update() {
		//Debug.Log("sanity " + sanity);
		//Debug.Log(sanityBar.GetComponentInChildren<Image>().fillAmount);
		if (Math.Abs(sanity - sanityTarget) > 0.01f) {
			float sanityDiff = Math.Min(Time.deltaTime*2.0f, Math.Abs(sanity - sanityTarget));
			if (sanity > sanityTarget) {
				sanityDiff *= -1.0f;
			}
			sanity += sanityDiff;
		}
		sanityBar.GetComponentInChildren<Image>().fillAmount=sanity/sanityMax;
		// heh ,this is lame
		if (sanity < 1.0f && !gameOverBool) {
			gameOver();
			gameOverFade();
		}

		if (fadeIn) {
			//Debug.Log("fadeIn");
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
				Text[] texts = fader.gameObject.GetComponentsInChildren<Text>();
				foreach(var img in texts) {
					img.color = new Color(1f, 1f, 1f, 1f - fadeTimeLeft / fadeTimeStart);
				}
			}
		}
		if (faded) {
			fadeTimeLeft -= Time.deltaTime;
			if (fadeTimeLeft < 0f) {
				faded = false;
				fadeOut = true;
				
				fadeTimeStart = 0.2f;
				fadeTimeLeft = 0.2f;
			}
		}
		if (fadeOut) {
			//Debug.Log("fadeOut " + fadeTimeLeft);
			fadeTimeLeft -= Time.deltaTime;
			if (fadeTimeLeft < 0f) {
				fader.color = new Color(0, 0, 0, 0);
				fadeOut = false;
				blockInput = false;
				newRoundPanel.SetActive(false);
			} else {
				// fade to black
				// there's nothing more for me, need the end to set me freeeee...
				fader.color = new Color(0, 0, 0, fadeTimeLeft / fadeTimeStart);
				Text[] texts = fader.gameObject.GetComponentsInChildren<Text>();
				foreach(var img in texts) {
					img.color = new Color(1f, 1f, 1f, fadeTimeLeft / fadeTimeStart);
				}
			}
			
		}
		if (totalFadeIn) {
			fadeTimeLeft -= Time.deltaTime;
			if (fadeTimeLeft < 0f) {
				fader.color = new Color(0, 0, 0, 1f);
				totalFadeIn = false;
			} else {
				// fade to transparent
				fader.color = new Color(0, 0, 0, 1f - fadeTimeLeft / fadeTimeStart);
				Text[] texts = fader.gameObject.GetComponentsInChildren<Text>();
				foreach(var img in texts) {
					img.color = new Color(1f, 1f, 1f, 1f - fadeTimeLeft / fadeTimeStart);
				}
			}
		}
	}

	public void gameOver() {
		gameOverBool = true;
	}
}

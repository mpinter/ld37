using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusicScript : MonoBehaviour {

	private bool inMenu;
	public AudioClip rain;
	public AudioClip music;
	private AudioSource audioSource;

	void Start () {
		Object.DontDestroyOnLoad(this);
		audioSource = this.GetComponent<AudioSource>();
		audioSource.loop = true;
		if (SceneManager.GetActiveScene().name == "MainMenu") {
			inMenu = true;
			audioSource.clip = rain;
			audioSource.Play();
		} else {
			inMenu = false;
			audioSource.clip = music;
			audioSource.Play();
		};
	}

	void Update () {
		if (SceneManager.GetActiveScene().name == "MainMenu" && !inMenu) {
			inMenu = true;
			audioSource.clip = rain;
			audioSource.Play();
		} 
		if (SceneManager.GetActiveScene().name != "MainMenu" && inMenu) {
			inMenu = false;
			audioSource.clip = music;
			audioSource.Play();
		};
	}
}

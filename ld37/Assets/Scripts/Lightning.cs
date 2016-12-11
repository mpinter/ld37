using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	Animator animator1;
	Animator animator2;
	Light svetlo;
	public float minWait;
	public float maxWait;
	float timeLeft;

	// Use this for initialization
	void Start () {
		animator1 = transform.Find("window1").GetComponent<Animator>();
		animator2 = transform.Find("window2").GetComponent<Animator>();
		svetlo = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			Debug.Log("blesk");
			if (Random.Range(0,2) == 1) {
				animator1.SetTrigger("Bum");
			} else {
				animator2.SetTrigger("Bum");
			}
			svetlo.intensity = 5.0f;
			timeLeft = Random.Range(minWait, maxWait);
		}
		if (svetlo.intensity > 0) {
			svetlo.intensity -= 0.05f;
		} else if (svetlo.intensity < 0) {
			svetlo.intensity = 0;
		}
	}
}

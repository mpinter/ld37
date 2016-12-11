using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dresser : MonoBehaviour {

	Animator animator;
	public float maxWait;
	public float minWait;
	float timeLeft;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			animator.SetTrigger("Nom");
			timeLeft = Random.Range(minWait, maxWait);
		}
	}
}

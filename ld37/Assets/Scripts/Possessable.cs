using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour {

	GameObject pentagram;
	GameObject svetlo;

	// Use this for initialization
	void Start () {
		pentagram = transform.Find("pentagram").gameObject;
		svetlo = transform.Find("light").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (tag == "Enemy" && !pentagram.activeSelf) {
			pentagram.SetActive(true);
			svetlo.SetActive(true);
		} else if (tag != "Enemy" && pentagram.activeSelf) {
			pentagram.SetActive(false);
			svetlo.SetActive(false);
		}
	}
}

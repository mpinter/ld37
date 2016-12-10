using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MapScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Logujem nieco ?");
		this.gameObject.GetComponent<InputScript>().Movement
		.Where(v => v != Vector2.zero)
		.Subscribe( todo => {
			Debug.Log("Hello");
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

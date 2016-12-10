using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MapScript : MonoBehaviour {

	public int width = 10;
	public int height = 10;

	public IObservable<Tile[][]> Tiles { get; private set; } 	

	// Use this for initialization
	void Start () {
		Debug.Log("Logujem nieco ?");
		this.gameObject.GetComponent<InputScript>().Movement
		.Where(v => v != Vector2.zero)
		.Subscribe( todo => {
			Debug.Log("Hello");
		})
		.AddTo(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

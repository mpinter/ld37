using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using flyyoufools;

public class MapScript : MonoBehaviour {

	public int width = 4;
	public int height = 4;

	private Tile[,] tiles = {{new Tile(),new Tile()}, {new Tile(),new Tile()}};

	public IObservable<Tile[,]> Tiles { get; private set; } 	

	// Use this for initialization
	void Start () {
		Tile a = new Tile();
		this.gameObject.GetComponent<InputScript>().Movement
		.Where(v => v != Vector2.zero)
		.Subscribe( todo => {
			Debug.Log("Hello");
		})
		.AddTo(this);
	}

	void move(int x, int y, MovementType type, int targetX, int targetY) {
		// WIP
	}

	void updateMap() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

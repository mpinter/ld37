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

	// move thing on x, y to target
	// checks everything it crashes into on the way and moves it if needed
	void move(int x, int y, MovementType type, int targetX, int targetY) {
		if (x != targetX && y != targetY) {
			throw new System.Exception("Should move along single axis");
		}
		if (!Helpers.inBounds(targetX, targetY, width, height)) return;
		int currentX = x;
		int currentY = y;
		// taget moved closer if we can't access it
		int currentTargetX = targetX;
		int currentTargetY = targetY;
		while (x != targetX && y != targetY) {
			// if we can push try move out of the way
			
			// try move
			
		}
	}

	void updateMap() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

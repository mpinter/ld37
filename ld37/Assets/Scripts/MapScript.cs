using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using flyyoufools;

public class MapScript : MonoBehaviour {

	public GameObject basicEnemy;

	public int width = 8;
	public int height = 8;

	private Tile[,] tiles;

	private char[] delims = {' '};

	public IObservable<Tile[,]> Tiles { get; private set; } 	

	private Tile getTile(string s) {
		var tile = new Tile();
		switch (s) {
			case "e":
				return new Tile();
			case "a":
				var enemy = Instantiate(basicEnemy) as GameObject;

		}
		return tile;
	}

	void Awake() {
		string[] currentMap = TestLevel.map;
		tiles = new Tile[height, width];
		for (int i = 0; i < height; ++i) {
			string[] elements = currentMap[i].Split(delims);
			for (int j = 0; j < width; ++j) {
				tiles[i,j] = getTile(elements[j]);
			}
		}
	}

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

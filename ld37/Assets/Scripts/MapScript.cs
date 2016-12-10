using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using flyyoufools;
using System;

public class MapScript : MonoBehaviour {

	public GameObject basicEnemy;

	public int width = 8;
	public int height = 8;
	
	private Tile[,] tiles;
	private int nextId = 0;

	private char[] delims = {' '};

	public ReactiveProperty<Tile[,]> Tiles { get; private set; } 	

	private Tile getTile(string s) {
		var tile = new Tile(flyyoufools.Action.Nothing, nextId++);
		switch (s) {
			case "e":
				break;
			case "a":
				var enemy = Instantiate(basicEnemy) as GameObject;
				tile.entity = enemy.GetComponent<Entity>();
				break;
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
		.Subscribe( vector => {
			
		})
		.AddTo(this);
	}

	// move thing on x, y to target
	// checks everything it crashes into on the way and moves it if needed
	void move(int row, int col, int targetCol, int targetRow) {
		if (col != targetCol && row != targetRow) {
			throw new System.Exception("Should move along single axis");
		}
		if (!Helpers.inBounds(targetCol, targetRow, width, height)) return;
		Entity entity = Tiles.Value[row, col].entity;
		int currentCol = col;
		int currentRow = row;
		// at this point expecting single axis of movement
		int incCol = (col != targetCol) ? 1 : 0;
		int incRow = (row != targetRow) ? 1 : 0;
		if (col > targetCol) incCol *= -1;
		if (row > targetRow) incRow *= -1;
		int numberOfSteps = Math.Abs(targetCol - col) + Math.Abs(targetRow - row);
		for (int i = 0; i < numberOfSteps; i++) {
			// if we can push try move out of the way
			bool moveSuccessful = false;
			Entity targetEntity = Tiles.Value[row+incRow, col+incCol].entity;
			if (targetEntity) {
			  	if (entity.canPush) {
					this.move(row+incRow, col+incCol, row+2*incRow, col+2*incCol);
					moveSuccessful = (Tiles.Value[row + incRow, col+incCol].entity == null) ? false : true;
			    } else if (entity.canTeleport) {
					//TODO later
				}
			} else {
				moveSuccessful = true;
			}
			// try move, or move target towards you
			if (moveSuccessful) {
			  currentCol += incCol;
			  currentRow += incRow; 
			}
		}
		// if we end up on same spot as another entity, do something ? todo
		Tiles.Value[row, col].entity = null;
		Tiles.Value[currentRow, currentCol].entity = entity; 
	}

	void updateMap() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

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

	private char[] delims = {' '};

	public ReactiveProperty<Tile[,]> Tiles { get; private set; } 	

	private Tile getTile(string s) {
		var tile = new Tile(flyyoufools.Action.Nothing);
		switch (s) {
			case "e":
				break;
			case "a":
				var enemy = Instantiate(basicEnemy) as GameObject;
				tile.entity = enemy.GetComponent<Entity>();
				break;
			case "p":
				var player = GameObject.FindWithTag("Player");
				tile.entity = player.GetComponent<Entity>();
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
		Tiles = new ReactiveProperty<Tile[,]>(tiles);
	}

	// Use this for initialization
	void Start () {
		Tile a = new Tile();
		this.gameObject.GetComponent<InputScript>().Movement
		.Where(v => v != Vector2.zero)
		.Throttle(TimeSpan.FromMilliseconds(1))
		.Subscribe( vector => {
			IntPair playerPosition = GameObject.FindWithTag("Player").GetComponent<Entity>().positionInTileSet(Tiles.Value);
			move(playerPosition.first, playerPosition.second, 
				playerPosition.first+(int)vector.y, playerPosition.second+(int)vector.x);
		})
		.AddTo(this);
	}

	// move thing on x, y to target
	// checks everything it crashes into on the way and moves it if needed
	void move(int row, int col, int targetRow, int targetCol) {
		if (col != targetCol && row != targetRow) {
			throw new System.Exception("Should move along single axis");
		};
		if (!Helpers.inBounds(targetRow, targetCol, height, width)) return;
		Entity entity = Tiles.Value[row, col].entity;
		int currentCol = col;
		int currentRow = row;
		// at this point expecting single axis of movement
		int incCol = (col != targetCol) ? 1 : 0;
		int incRow = (row != targetRow) ? 1 : 0;
		int lastTeleportCol = col;
		int lastTeleportRow = row;
		if (col > targetCol) incCol *= -1;
		if (row > targetRow) incRow *= -1;
		int numberOfSteps = Math.Abs(targetCol - col) + Math.Abs(targetRow - row);
		for (int i = 0; i < numberOfSteps; i++) {
			// if we can push try move out of the way
			bool moveSuccessful = false;
			Entity targetEntity = Tiles.Value[row+incRow, col+incCol].entity;
			if (targetEntity) {
			  	if (entity.canPush) {
					this.move(currentRow+incRow, currentCol+incCol, currentRow+2*incRow, currentCol+2*incCol);
					moveSuccessful = (Tiles.Value[currentRow + incRow, currentCol+incCol].entity == null) ? true : false;
			    } else if (entity.canTeleport) {
					// 'move' without succesfull teleport
					currentCol += incCol;
					currentRow += incRow;
				}
			} else {
				moveSuccessful = true;
			}
			// try move, or move target towards you
			if (moveSuccessful) {
			  currentCol += incCol;
			  currentRow += incRow;
			  lastTeleportCol = col;
			  lastTeleportRow = row; 
			}
		}
		if (entity.canTeleport) {
			currentCol = lastTeleportCol;
			currentRow = lastTeleportRow;
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

	private bool inTileMap(int row, int col) {
		return row >= 0 && row < height && col >= 0 && col < width;
	}

	private List<Helpers.IntPos> NearestPath(int from_row, int from_col, int to_row, int to_col) {
		if (from_row == to_row && from_col == to_col) {
			return new List<Helpers.IntPos>();
		}
		bool[,] visited = new bool[height, width]; 
		Queue<Helpers.BfsPos> queue = new Queue<Helpers.BfsPos>();
		visited[from_row, from_col] = true;
		queue.Enqueue(new Helpers.BfsPos(from_row, from_col, null));

		while (queue.Count > 0) {
			var top = queue.Dequeue();
			if (top.row == to_row && top.col == to_col) {
				queue.Enqueue(top);
				break;
			}
			visited[top.row, top.col] = true;
			for (int i = -1; i <= 1; ++i) {
				for (int j = -1; j <= 1; ++j) {
					if ((i == 0 || j == 0) && !(i==j)) {
						var r = top.row + i;
						var c = top.col + j;
						if (inTileMap(r, c) && (tiles[r, c].entity != null) && !tiles[r,c].entity.isWall) {
							// valid pos
							queue.Enqueue(new Helpers.BfsPos(r, c, top));
						}
					}
				}
 			}
		}
		if (queue.Count == 0) {
			// target not accessible
			return new List<Helpers.IntPos>();
		}
		var curr = queue.Dequeue();
		if (!(curr.row == to_row && curr.col == to_col)) {
			// target not accessible
			return new List<Helpers.IntPos>();
		}
		// build path back to start
		List<Helpers.IntPos> path = new List<Helpers.IntPos>();
		while (curr.prev != null) {
			path.Add(new Helpers.IntPos(curr));
			curr = curr.prev;
		}
		return path;
	}

}

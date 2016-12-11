using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using flyyoufools;
using System;

public class MapScript : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject chasingPrefab;
	public GameObject chargingPrefab;
	public GameObject rookPrefab;
	public GameObject wallPrefab;

	public int width = 8;
	public int height = 8;
	
	private Tile[,] tiles;

	private char[] delims = {' '};

	public ReactiveProperty<Tile[,]> Tiles { get; private set; } 

	//private Subject<Tile[,]> tilesChanged;
	/*public IObservable<Tile[,]> TilesChanged {
		get { return tilesChanged; }
	}*/	
	private Entity playerEntity;

	private Tile getTile(string s) {
		var tile = new Tile(flyyoufools.Action.Nothing);
		GameObject instantiatedObject;
		switch (s) {
			case "e":
				break;
			case "p":
				instantiatedObject = Instantiate(playerPrefab) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				break;
			case "w":
				instantiatedObject = Instantiate(wallPrefab) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				break;
			case "f":
				instantiatedObject = Instantiate(chasingPrefab) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				break;
			case "c":
				instantiatedObject = Instantiate(chargingPrefab) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				break;
			case "r":
				instantiatedObject = Instantiate(rookPrefab) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
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
		//tilesChanged = new Subject<Tile[,]>().AddTo(this);
	}

	// Use this for initialization
	void Start () {
		playerEntity = GameObject.FindWithTag("Player").GetComponent<Entity>();
		this.gameObject.GetComponent<InputScript>().Movement
		.Subscribe( vector => {
			Helpers.IntPos playerPosition = playerEntity.positionInTileSet(Tiles.Value);
			move(playerPosition.row, playerPosition.col, 
				playerPosition.row + (int)vector.y, playerPosition.col + (int)vector.x);
		})
		.AddTo(this);
		//tilesChanged.OnNext(tiles);
	}

	private List<Helpers.IntPos> getNextPosition(Entity entity, Helpers.IntPos currentPos) {
		Helpers.IntPos playerPos = GameObject.FindWithTag("Player").GetComponent<Entity>().positionInTileSet(Tiles.Value);
		List<Helpers.IntPos> retList = new List<Helpers.IntPos>();
		switch(entity.entityType) {
			case EntityType.ChargingEnemy:
				Debug.Log("Charging emeny moving");
				switch (entity.chargingDirection) {
					case 0:
						retList.Add(new Helpers.IntPos(0, currentPos.col));
						break;
					case 1:
						retList.Add(new Helpers.IntPos(currentPos.row, width-1));
						break;
					case 2:
						retList.Add(new Helpers.IntPos(height-1, currentPos.col));
						break;
					case 3:
						retList.Add(new Helpers.IntPos(currentPos.row, 0));
						break;
				}
				entity.chargingDirection = UnityEngine.Random.Range(0,4);
			  	break;
			case EntityType.ChasingEnemy:
				Debug.Log("Chasing enemy moving");
				retList = NearestPath(currentPos.row, currentPos.col, playerPos.row, playerPos.col);
				retList = retList.GetRange(0,Math.Min(3, retList.Count));
				break;
			case EntityType.RookEnemy:
				Debug.Log("Rook emeny moving");
				if (entity.rookState) {
					retList.Add(new Helpers.IntPos(currentPos.row, playerPos.col));
				} else {
					retList.Add(new Helpers.IntPos(playerPos.row, currentPos.col));
				}
				entity.rookState = !entity.rookState;
			  	break;
		}
		return retList;
	}

	public void enemyTurn() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		// TODO damage
		foreach (GameObject enemy in enemies) {
			Helpers.IntPos currentPos = enemy.GetComponent<Entity>().positionInTileSet(Tiles.Value);
			getNextPosition(enemy.GetComponent<Entity>(), currentPos).ForEach((pos) => {
				move(currentPos.row, currentPos.col, pos.row, pos.col);
				currentPos = pos;
			});
		}
	}

	// move thing on x, y to target
	// checks everything it crashes into on the way and moves it if needed
	void move(int row, int col, int targetRow, int targetCol) {
		var testWtf = (Tile[,])Tiles.Value.Clone();
		if (col != targetCol && row != targetRow) {
			Debug.Log("Should move along single axis!!!!!!!!!!");
			throw new System.Exception("Should move along single axis");
		};
		if (!Helpers.inBounds(targetRow, targetCol, height, width)) return;
		Entity entity = testWtf[row, col].entity;
		int currentCol = col;
		int currentRow = row;
		// at this point expecting single axis of movement
		int incCol = (col != targetCol) ? 1 : 0;
		int incRow = (row != targetRow) ? 1 : 0;
		int lastFreeCol = col;
		int lastFreeRow = row;
		if (col > targetCol) incCol *= -1;
		if (row > targetRow) incRow *= -1;
		int numberOfSteps = Math.Abs(targetCol - col) + Math.Abs(targetRow - row);
		for (int i = 0; i < numberOfSteps; i++) {
			// if we can push try move out of the way
			bool moveSuccessful = false;
			Entity targetEntity = testWtf[row+incRow, col+incCol].entity;
			if (targetEntity) {
			  	if (entity.canPush) {
					this.move(currentRow+incRow, currentCol+incCol, currentRow+2*incRow, currentCol+2*incCol);
					moveSuccessful = (testWtf[currentRow + incRow, currentCol+incCol].entity == null) ? true : false;
					if (moveSuccessful) {
						if (targetEntity.gameObject.CompareTag("Enemy")) {
							GameObject.FindWithTag("Master").GetComponent<MasterScript>().movePossesed();
						} else {
							GameObject.FindWithTag("Master").GetComponent<MasterScript>().moveUnpossesed();
						}
					}
			    } 
				if (
					(entity.gameObject.CompareTag("Enemy")) && 
					(targetEntity.entityType != EntityType.Wall || entity.canTeleport)
				) {
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
			  lastFreeCol = currentCol;
			  lastFreeRow = currentRow; 
			}
		}
		// if we end up on same spot as another entity, do something ? todo
		testWtf[row, col].entity = null;
		if (testWtf[currentRow, currentCol].entity != null) {
			entity.Destroy();
			testWtf[currentRow, currentCol].entity.Destroy();
		}
		if (row != lastFreeRow || col != lastFreeCol) {
			// possibly more things to do if actually moved
			if (entity.gameObject.CompareTag("Player")) {
				GameObject.FindWithTag("Master").GetComponent<MasterScript>().movePlayer();
			}
		}
		testWtf[lastFreeRow, lastFreeCol].entity = entity;
		testWtf[lastFreeRow, lastFreeCol].lastAction = flyyoufools.Action.Move;
		Tiles.Value = testWtf;
		//tilesChanged.OnNext(tiles);
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
		// already initialized to false
		bool[,] visited = new bool[height, width]; 
		Queue<Helpers.BfsPos> queue = new Queue<Helpers.BfsPos>();
		queue.Enqueue(new Helpers.BfsPos(from_row, from_col, null));

		int[,] deltas = {
			{-1, 0},
			{1, 0},
			{0, -1},
			{0, 1}
		};

		Helpers.BfsPos found = null;
		while (queue.Count > 0) {
			var top = queue.Dequeue();
			if (top.row == to_row && top.col == to_col) {
				// we have found target
				found = top;
				break;
			}
			for (int d = 0; d < 4; ++d) {
				var r = top.row + deltas[d,0];
				var c = top.col + deltas[d,1];
				if (inTileMap(r, c) && !visited[r, c] && ((tiles[r,c].entity == null) || !(tiles[r,c].entity.entityType == EntityType.Wall))) {
					// valid pos
					visited[r, c] = true;
					queue.Enqueue(new Helpers.BfsPos(r, c, top));
				}
			}
		}
		if (found == null) {
			// target not accessible
			return new List<Helpers.IntPos>();
		}
		// build path back to start
		List<Helpers.IntPos> path = new List<Helpers.IntPos>();
		while (found.prev != null) {
			path.Add(new Helpers.IntPos(found));
			found = found.prev;
		}
		path.Add(new Helpers.IntPos(found));
		path.Reverse();
		return path;
	}

}

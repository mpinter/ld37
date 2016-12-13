using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using flyyoufools;
using System;

public class MapScript : MonoBehaviour {

	public int mapId = 0;
	public GameObject playerPrefab;
	public GameObject chasingPrefab;
	public GameObject chasingPrefab2;
	public GameObject chargingPrefab;
	public GameObject chargingPrefab2;
	public GameObject rookPrefab;
	public GameObject rookPrefab2;
	public GameObject wallPrefab;
	public GameObject wallPrefab2;
	public GameObject highlight;

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
			case "W":
				instantiatedObject = Instantiate(wallPrefab2) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				break;
			case "f":
			case "F":
				instantiatedObject = UnityEngine.Random.Range(0,2) == 1 ? Instantiate(chasingPrefab) : Instantiate(chasingPrefab2) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				instantiatedObject.tag = (s == "F") ? "Enemy" : "Wall";
				break;
			case "c":
			case "C":			
 				instantiatedObject = UnityEngine.Random.Range(0,2) == 1 ? Instantiate(chargingPrefab) : Instantiate(chargingPrefab2) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				instantiatedObject.tag = (s == "C") ? "Enemy" : "Wall";
				instantiatedObject.GetComponent<Entity>().chargingDirection = (s == "C") ? UnityEngine.Random.Range(0,4) : 2;
				instantiatedObject.GetComponent<Animator>().SetInteger("Direction", instantiatedObject.GetComponent<Entity>().chargingDirection);
				break;
			case "r":
			case "R":
				instantiatedObject = UnityEngine.Random.Range(0,2) == 1 ? Instantiate(rookPrefab) : Instantiate(rookPrefab2) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				tile.entity.rookState = false;
				instantiatedObject.tag = (s == "R") ? "Enemy" : "Wall";
				var tmp = (s == "R") ? 2 : -1;
				instantiatedObject.GetComponent<Animator>().SetInteger("Direction", tmp);
				break;
			case "x":
			case "X":
				instantiatedObject = Instantiate(rookPrefab) as GameObject;
				tile.entity = instantiatedObject.GetComponent<Entity>();
				tile.entity.rookState = true;
				instantiatedObject.tag = (s == "X") ? "Enemy" : "Wall";
				var tmp2 = (s == "X") ? 1 : -1;
				instantiatedObject.GetComponent<Animator>().SetInteger("Direction", tmp2);
				break;
		}
		return tile;
	}

	void Awake() {
		// mapId - 0..8
		mapId = PlayerPrefs.GetInt("CurrentLevel");
		string[] currentMap = TestLevel.maps[mapId % TestLevel.maps.Count];

		tiles = new Tile[height, width];
		for (int i = 0; i < height; ++i) {
			string[] elements = currentMap[i].Split(delims);
			for (int j = 0; j < width; ++j) {
				tiles[i,j] = getTile(elements[j]);
				// instantiate highlight and turn it off
				var obj = Instantiate(highlight) as GameObject;
				obj.transform.position =  new Vector3(-5.3f + 0.6f + j * (192f/144f), 3f - 0.5f - i, 0.5f);
				obj.GetComponentInChildren<SpriteRenderer>().enabled = false;
			}
		}
		Tiles = new ReactiveProperty<Tile[,]>(tiles);
		//tilesChanged = new Subject<Tile[,]>().AddTo(this);
	}

	// Use this for initialization
	void Start () {
		this.GetComponent<AudioSource>().Stop();
		playerEntity = GameObject.FindWithTag("Player").GetComponent<Entity>();
		var nextLevelButton = GameObject.Find("NextButton").GetComponent<Button>();
		var levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
		nextLevelButton.onClick.AddListener(delegate{levelLoader.LoadLevel("Level"+(mapId+1));});
		if (mapId < TestLevel.texts.Count) {
			GameObject.Find("IntroText").GetComponent<Text>().text = TestLevel.texts[Math.Min(mapId, TestLevel.texts.Count)];
		}
		this.gameObject.GetComponent<InputScript>().Movement
		.Subscribe( vector => {
			var testWtf = (Tile[,])Tiles.Value.Clone();			
			Helpers.IntPos playerPosition = playerEntity.positionInTileSet(Tiles.Value);
			move(playerPosition.row, playerPosition.col, 
				playerPosition.row + (int)vector.y, playerPosition.col + (int)vector.x, true, testWtf);
			// just to be sure, more resilent if we clear possible errors
			for (int r = 0; r <= testWtf.GetUpperBound(0); ++r) {
				for (int c = 0; c <= testWtf.GetUpperBound(1); ++c) {
					if (testWtf[r,c].waitingEntities.Count > 0) {
						Debug.Log("STUCKED ENTITIES ON PLAYER MOVE - ERROR!!");
						for (int i=0; i<testWtf[r,c].waitingEntities.Count; i++) {
							testWtf[r,c].waitingEntities[i].Destroy(null);
							Destroy(testWtf[r,c].waitingEntities[i].gameObject);
						}
						testWtf[r,c].waitingEntities.Clear();
						testWtf[r,c].reverseMoveVector.Clear();
					}
				}
			}
			Tiles.Value = testWtf;
		})
		.AddTo(this);
		//tilesChanged.OnNext(tiles);
	}

	private List<Helpers.IntPos> getNextPosition(Entity entity, Helpers.IntPos currentPos, bool mutable = true) {
		Helpers.IntPos playerPos = GameObject.FindWithTag("Player").GetComponent<Entity>().positionInTileSet(Tiles.Value);
		List<Helpers.IntPos> retList = new List<Helpers.IntPos>();
		var animator = entity.GetComponent<Animator>();
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
				if (mutable) {
					entity.chargingDirection = UnityEngine.Random.Range(0,4);
					animator.SetInteger("Direction", entity.chargingDirection);
				}
			  	break;
			case EntityType.ChasingEnemy:
				Debug.Log("Chasing enemy moving");
				retList = NearestPath(currentPos.row, currentPos.col, playerPos.row, playerPos.col);
				retList = retList.GetRange(0,Math.Min(4, retList.Count));
				break;
			case EntityType.RookEnemy:
				Debug.Log("Rook emeny moving");
				if (entity.rookState) {
					retList.Add(new Helpers.IntPos(currentPos.row, playerPos.col));
				} else {
					retList.Add(new Helpers.IntPos(playerPos.row, currentPos.col));
				}
				if (mutable) {
					entity.rookState = !entity.rookState;
					if (entity.rookState) {
						animator.SetInteger("Direction", currentPos.col - playerPos.col > 0 ? 3 : 1);
					} else {
						animator.SetInteger("Direction", currentPos.row - playerPos.row > 0 ? 0 : 2);
					}
				}
			  	break;
		}
		return retList;
	}

	public Tile[,] tilemapAfterTurn() {
		// THIS CODE IS COPYPASTED IN enemyTurn, change it ALSO THERE
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		// deep copy of testWtf
		var oldTiles = Tiles.Value;
		var testWtf = new Tile[Tiles.Value.GetUpperBound(0)+1, Tiles.Value.GetUpperBound(1)+1];
		for (int r = 0; r <= testWtf.GetUpperBound(0); ++r) {
			for (int c = 0; c <= testWtf.GetUpperBound(1); ++c) {
				if (oldTiles[r,c] == null) {
					continue;
				}
				testWtf[r,c] = new Tile();
				testWtf[r,c].lastAction = oldTiles[r,c].lastAction;
				testWtf[r,c].entity = oldTiles[r,c].entity;
				// maybe copy waitingEntities and reverseMoveVector?
			}
		}
		
		foreach (GameObject enemy in enemies) {
			Helpers.IntPos currentPos = enemy.GetComponent<Entity>().positionInTileSet(Tiles.Value);
			// not mutable getNextPosition
			getNextPosition(enemy.GetComponent<Entity>(), currentPos, false).ForEach((pos) => {
				move(currentPos.row, currentPos.col, pos.row, pos.col,false, testWtf, false);
				currentPos = pos;
			});
		}
		// resolve collisions
		for (int r = 0; r <= testWtf.GetUpperBound(0); ++r) {
			for (int c = 0; c <= testWtf.GetUpperBound(1); ++c) {
				if (testWtf[r,c].waitingEntities.Count > 0) {
					//all good if single and place is free
					if (testWtf[r,c].entity == null || testWtf[r,c].entity.entityType == EntityType.Player) {
						continue;
					}
					if (testWtf[r,c].waitingEntities.Count == 1 && testWtf[r,c].entity==null) {
						testWtf[r,c].entity = testWtf[r,c].waitingEntities[0];
					} else {
						
						for (int i=0; i<testWtf[r,c].waitingEntities.Count; i++) {
							findFirstFreeInVectorAndUnpossess(testWtf[r,c].waitingEntities[i], r,c, testWtf[r,c].reverseMoveVector[i], testWtf, false);
						}
						testWtf[r,c].entity = null;
						// testWtf[r,c].entity.Destroy(testWtf[r,c].waitingEntities[0].entityType);
					}
					// testWtf[r,c].waitingEntities.Clear();
					// testWtf[r,c].reverseMoveVector.Clear();
				}
			}
		} 
		return testWtf;
	}
	public void enemyTurn() {
		// THIS CODE IS COPYPASTED IN tilemapAfterTurn, change it ALSO THERE
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		var testWtf = (Tile[,])Tiles.Value.Clone();
		foreach (GameObject enemy in enemies) {
			Helpers.IntPos currentPos = enemy.GetComponent<Entity>().positionInTileSet(Tiles.Value);
			getNextPosition(enemy.GetComponent<Entity>(), currentPos).ForEach((pos) => {
				move(currentPos.row, currentPos.col, pos.row, pos.col,false, testWtf);
				currentPos = pos;
			});
		}
		//resolve collisions
		for (int r = 0; r <= testWtf.GetUpperBound(0); ++r) {
			for (int c = 0; c <= testWtf.GetUpperBound(1); ++c) {
				if (testWtf[r,c].waitingEntities.Count > 0) {
					//all good if single and place is free
					if (testWtf[r,c].waitingEntities.Count == 1 && testWtf[r,c].entity==null) {
						testWtf[r,c].entity = testWtf[r,c].waitingEntities[0];
					} else {
						for (int i=0; i<testWtf[r,c].waitingEntities.Count; i++) {

							findFirstFreeInVectorAndUnpossess(testWtf[r,c].waitingEntities[i], r,c, testWtf[r,c].reverseMoveVector[i], testWtf);
						}
						testWtf[r,c].entity.Destroy(testWtf[r,c].waitingEntities[0].entityType);
						if (!this.GetComponent<AudioSource>().isPlaying) this.GetComponent<AudioSource>().Play();
					}
					testWtf[r,c].waitingEntities.Clear();
					testWtf[r,c].reverseMoveVector.Clear();
				}
			}
		} 
		Tiles.Value = testWtf;
	}

	

	// move thing on x, y to target
	// checks everything it crashes into on the way and moves it if needed
	void move(int row, int col, int targetRow, int targetCol, bool isPlayerInteraction, Tile[,] testWtf, bool mutable = true) {
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
			Entity targetEntity = testWtf[currentRow+incRow, currentCol+incCol].entity;
			if (targetEntity != null) {
			  	if (entity.canPush && targetEntity.entityType != EntityType.ImovableWall) {
					this.move(currentRow+incRow, currentCol+incCol, currentRow+2*incRow, currentCol+2*incCol, isPlayerInteraction, testWtf, mutable);
					moveSuccessful = (testWtf[currentRow + incRow, currentCol+incCol].entity == null) ? true : false;
					if (moveSuccessful) {
						if (targetEntity.gameObject.CompareTag("Enemy")) {
							if (mutable) {
								GameObject.FindWithTag("Master").GetComponent<MasterScript>().movePossesed();
							}
						} else {
							if (mutable) {
								GameObject.FindWithTag("Master").GetComponent<MasterScript>().moveUnpossesed();
							}
						}
					}
			    } 
				if (
					!isPlayerInteraction &&
					(entity.gameObject.CompareTag("Enemy")) 
				) {
					if (!targetEntity.gameObject.CompareTag("Wall"))
						{
							moveSuccessful = true;
						} else if (entity.canTeleport) {
							currentCol += incCol;
							currentRow += incRow;
						}
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
		if (row != lastFreeRow || col != lastFreeCol) {
			// possibly more things to do if actually moved
			if (entity.gameObject.CompareTag("Player")) {
				if (mutable) {
					GameObject.FindWithTag("Master").GetComponent<MasterScript>().movePlayer();
				}
			}
		}
		if (testWtf[currentRow, currentCol].entity != null && 
           (testWtf[currentRow, currentCol].entity.gameObject.CompareTag("Player") || testWtf[currentRow, currentCol].entity.gameObject.CompareTag("Enemy"))
		) {
			testWtf[lastFreeRow, lastFreeCol].waitingEntities.Add(entity);
			testWtf[lastFreeRow, lastFreeCol].reverseMoveVector.Add(new Helpers.IntPos(-incRow, -incCol));
		} else {
			testWtf[lastFreeRow, lastFreeCol].entity = entity;
		}
		// this is bit unsafe, but assume that within single movement cycle only one type of movement
		// is used anyway
		testWtf[lastFreeRow, lastFreeCol].lastAction = flyyoufools.Action.Move;
		//tilesChanged.OnNext(tiles);
	}

	void findFirstFreeInVectorAndUnpossess(Entity entity, int row, int col, Helpers.IntPos targetVector, Tile[,] testWtf, bool mutable = true) {
		int currentCol = col;
		int currentRow = row;
		bool success = false;
		while (Helpers.inBounds(currentRow, currentCol, height, width)) {
			if (testWtf[currentRow, currentCol].entity == null || testWtf[currentRow, currentCol].entity == entity) {
				testWtf[currentRow, currentCol].entity = entity;
				success = true;
				break;
			}
			currentCol+=targetVector.col;
			currentRow+=targetVector.row;
		}
		if (mutable) {
			if (!success) {
				// emergency save
				Debug.Log("Had to destroy, no place");
				Destroy(entity.gameObject);
			} else {
				entity.Destroy(null);
			}
		}
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
				if (inTileMap(r, c) && !visited[r, c] && ((tiles[r,c].entity == null) || !(tiles[r,c].entity.gameObject.CompareTag("Wall")))) {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using flyyoufools;

public class Entity : MonoBehaviour {
	public int id { get; set; }

	int lastRow, lastCol;
	Action lastAction;
	int currentRow, currentCol;

	public EntityType entityType;

	public bool canPush;
	public bool canTeleport;

	// switches each round
	private bool rookState = false;

	// Use this for initialization
	void Start () {
		GameObject master = GameObject.FindGameObjectWithTag("Master");
		MapScript mapScript = master.GetComponent<MapScript>();
		var tileObservable = mapScript.Tiles;
		tileObservable.Subscribe( tileMap => {
			Debug.Log("Entity TileMap changed");
			tilesChanged(tileMap);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<IntPos> getNextPosition() {
		//TODO
	}

	public IntPair positionInTileSet(Tile[,] tileSet) {
		for (int r = 0; r <= tileSet.GetUpperBound(0); ++r) {
			for (int c = 0; c <= tileSet.GetUpperBound(1); ++c) {
				if (!tileSet[r,c].entity) continue;
				if (tileSet[r,c].entity.gameObject == this.gameObject) {
					return new IntPair(r,c);
				}
			}
		} 
		return null;
	}

	protected void tilesChanged(Tile[,] tileSet) {
		var found = positionInTileSet(tileSet);
		if (found == null) {
			return;
		}
		currentRow = found.first;
		currentCol = found.second;
		var currentAction = tileSet[currentRow,currentCol].lastAction;
		// TOOD: do something
		
		// save new state
		lastAction = currentAction;
		lastRow = currentRow;
		lastCol = currentCol;
	} 
}
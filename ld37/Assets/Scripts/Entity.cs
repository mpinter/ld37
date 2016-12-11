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
	public bool rookState = false;
	// 0 - 3, from 12 o'clock clockwise
	public int chargingDirection = 0;

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

	public Helpers.IntPos positionInTileSet(Tile[,] tileSet) {
		for (int r = 0; r <= tileSet.GetUpperBound(0); ++r) {
			for (int c = 0; c <= tileSet.GetUpperBound(1); ++c) {
				if (!tileSet[r,c].entity) continue;
				if (tileSet[r,c].entity.gameObject == this.gameObject) {
					return new Helpers.IntPos(r,c);
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
		currentRow = found.row;
		currentCol = found.col;
		var currentAction = tileSet[currentRow,currentCol].lastAction;
		// TOOD: do something
		
		// save new state
		lastAction = currentAction;
		lastRow = currentRow;
		lastCol = currentCol;
	} 

	public void Destroy() {
		if (entityType == EntityType.Player) {
			Debug.Log("Game Over");
		} else {
			// assuming we're not calling this on walls
			Debug.Log("Destroyed something possesed, should respawn, TODO");
		}
	}
}
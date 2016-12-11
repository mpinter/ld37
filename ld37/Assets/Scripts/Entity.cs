using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using flyyoufools;

public class Entity : MonoBehaviour {
	public int id { get; set; }
	// TODO: fill somewhere
	public bool isWall;

	int lastRow, lastCol;
	Action lastAction;
	int currentRow, currentCol;

	public bool canPush;
	public bool canTeleport;

	// Use this for initialization
	void Start () {
		GameObject master = GameObject.FindGameObjectWithTag("Master");
		MapScript mapScript = master.GetComponent<MapScript>();
		var tileObservable = mapScript.Tiles;
		tileObservable.Subscribe( tileMap => {
			Debug.Log("Entity TileMap changed");
			tilesChanged(tileMap);
		}).AddTo(this);
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
		transform.position = new Vector2(-5.3f + (1.3f/2f) + currentCol * 1.3f, 3f - 0.5f - currentRow); 
		
		// save new state
		lastAction = currentAction;
		lastRow = currentRow;
		lastCol = currentCol;
	} 
}
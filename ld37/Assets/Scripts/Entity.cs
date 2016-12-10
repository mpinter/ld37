using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using flyyoufools;

public class Entity : MonoBehaviour {
	public int id { get; set; }

	int lastRow, lastCol;
	Action lastAction;
	int currentRow, currentCol;

	// Use this for initialization
	void Start () {
		GameObject master = GameObject.FindGameObjectWithTag("Master");
		MapScript mapScript = master.GetComponent<MapScript>();
		var tileObservable = mapScript.Tiles;
		tileObservable.Subscribe( tileMap => {
			Debug.Log("Entity TileMap changed");
			tilesChanged(tileMap);
		})
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	bool positionInTileSet(Tile[][] tileSet) {
		for (int r = 0; r < tileSet.Length; ++r) {
			for (int c = 0; c < tileSet[r].Length; ++c) {
				if (tileSet[r][c].entityId == id) {
					currentRow = r;
					currentCol = c;
					return true;
				}
			}
		} 
		return false;
	}

	protected void tilesChanged(Tile[][] tileSet) {
		var found = positionInTileSet(tileSet);
		if (!found) {
			return;
		}
		var currentAction = tileSet[currentRow][currentCol].lastAction;
		// TOOD: do something

		// save new state
		lastAction = currentAction;
		lastRow = currentRow;
		lastCol = currentCol;
	} 
}
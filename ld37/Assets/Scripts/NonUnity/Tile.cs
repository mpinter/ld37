using System.Collections;
using System.Collections.Generic;

using flyyoufools;

public class Tile {

	public Action lastAction;
	public int? entityId;

	public Tile(Action action = Action.Nothing, int? id = null) {
		lastAction = action;
		entityId = id;
	}
}

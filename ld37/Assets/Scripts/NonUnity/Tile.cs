using System.Collections;
using System.Collections.Generic;

using flyyoufools;

public class Tile {

	Action lastAction;
	int? entityId;

	public Tile(Action action = Action.Nothing, int? id = null) {
		lastAction = action;
		entityId = id;
	}
}

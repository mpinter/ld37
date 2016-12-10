using System.Collections;
using System.Collections.Generic;

using flyyoufools;

public class Tile {

	public Action lastAction;
	public Entity entity;

	public Tile(Action action = Action.Nothing, Entity entity = null) {
		lastAction = action;
		entity = entity;
	}
}

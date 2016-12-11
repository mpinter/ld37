using System.Collections;
using System.Collections.Generic;

using flyyoufools;

public class Tile {

	public Action lastAction;
	public Entity entity;
	//next two lists should be coupled
	public List<Entity> waitingEntities;
	public List<Helpers.IntPos> reverseMoveVector;

	public Tile(Action action = Action.Nothing, Entity _entity = null) {
		lastAction = action;
		entity = _entity;
		waitingEntities = new List<Entity>();
		reverseMoveVector = new List<Helpers.IntPos>();
	}
}

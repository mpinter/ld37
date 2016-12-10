using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using flyyoufools;
using System;

public class MapScript : MonoBehaviour {

	public int width = 4;
	public int height = 4;

	// {{new Tile(),new Tile()}, {new Tile(),new Tile()}};

	public ReactiveProperty<Tile[,]> Tiles { get; private set; } 	

	// Use this for initialization
	void Start () {
		Tile a = new Tile();
		this.gameObject.GetComponent<InputScript>().Movement
		.Where(v => v != Vector2.zero)
		.Subscribe( todo => {
			Debug.Log("Hello");
		})
		.AddTo(this);
	}

	// move thing on x, y to target
	// checks everything it crashes into on the way and moves it if needed
	void move(int x, int y, int targetX, int targetY) {
		if (x != targetX && y != targetY) {
			throw new System.Exception("Should move along single axis");
		}
		if (!Helpers.inBounds(targetX, targetY, width, height)) return;
		Entity entity = Tiles.Value[x,y].entity;
		int currentX = x;
		int currentY = y;
		// at this point expecting single axis of movement
		int incX = (x != targetX) ? 1 : 0;
		int incY = (y != targetY) ? 1 : 0;
		if (x > targetX) incX *= -1;
		if (y > targetY) incX *= -1;
		int numberOfSteps = Math.Abs(targetX - x) + Math.Abs(targetY - y);
		for (int i = 0; i < numberOfSteps; i++) {
			// if we can push try move out of the way
			bool moveSuccessful = false;
			Entity targetEntity = Tiles.Value[x+incX, y+incY].entity;
			if (targetEntity) {
			  	if (entity.canPush) {
					this.move(x+incX, y+incY, x+2*incX, y+2*incY);
					moveSuccessful = (Tiles.Value[x+incX, y+incY].entity == null) ? false : true;
			    } else if (entity.canTeleport) {
					//TODO later
				}
			} else {
				moveSuccessful = true;
			}
			// try move, or move target towards you
			if (moveSuccessful) {
			  currentX += incX;
			  currentY += incY; 
			}
		}
		// if we end up on same spot as another entity, do something ? todo
		Tiles.Value[x,y].entity = null;
		Tiles.Value[currentX, currentY].entity = entity; 
	}

	void updateMap() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class TilePoint: IHasNeighbours<TilePoint> {

	public int x = 0;
	public int y = 0;
	
	public Path<TilePoint> zeroPath;
	public HexaShip ship;
	public int Count = 0;
	
	public bool Passable = true;
	
	public TilePoint(int x, int y):base() {
		
		this.x = x;
		this.y = y;
		
		Passable = true;
	}
	public string index {
		get { return x + ":" + y; }
	}
	public IEnumerable<TilePoint> AllNeighbours { get; set; }
	public IEnumerable<TilePoint> Neighbours {
		get { return AllNeighbours.Where(o => o.Passable); }
	}
	public void getNeibors() {
		int offsetX = Mathf.Abs(y % 2);
		List<TilePoint> neibors = new List<TilePoint>(); 
		List<TilePoint> shift = new List<TilePoint> {
			new TilePoint(x + 1 , y + 0),
			new TilePoint(x + -1, y + 0),
			new TilePoint(x + 0 + offsetX, y + 1),
			new TilePoint(x + -1 + offsetX, y + 1),
			new TilePoint(x + -1 + offsetX, y + -1),
			new TilePoint(x + 0 + offsetX, y + -1),
		};
		Count = 0;
		foreach (TilePoint point in shift) {
			
			if (ship.tileSet.ContainsKey(point.index)) {
				Count++;
				neibors.Add(ship.tileSet[point.index].key);
	   	}
    }
		AllNeighbours = neibors;
 	}
 	
	public bool pathToZero() {
		zeroPath = PathFinder.FindPath(ship.zero, this);
		return (zeroPath != null);
	}
	
	public Vector3 Vector() {
		HexaTile tile = ship.GetTile(index);
		if (tile == null) return Vector3.zero;
		return tile.transform.position;
	}
	
	override public string ToString() {
		return "TILE: " + index;
	}
}

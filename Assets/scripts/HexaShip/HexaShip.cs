﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class HexaShip : MonoBehaviour {

	public Dictionary<string, HexaTile> tileSet = new Dictionary<string, HexaTile>();
	public Dictionary<TilePoint, int> shipData;
	
	public TilePoint zero = null;
	public TilePoint tractor = null;
	
	
	public static HexaShip createShip(Dictionary<TilePoint, int> data, Vector3 position) {
	
		HexaShip ship = (Instantiate (Resources.Load ("Ship"),position,Quaternion.identity)as GameObject).GetComponent<HexaShip>();
			ship.shipData = data;
		
		return ship;	
	}
	
	// Use this for initialization
	void Start () {
		
		foreach (KeyValuePair<TilePoint, int> tile in shipData) {
			createTile(tile.Key, tile.Value);
		}	
		
		RecalcShip();
		RecalcEnergy();
  }
	
	public bool createTile(int x, int y, int tileID, bool check = false) {
		TilePoint key = new TilePoint(x,y);
		if (tileSet.ContainsKey(key.index)) {
			Debug.Log("Existing index " + key.index);
			return false;
		}
		key.ship = this;
		
		Vector2 hexpos = ShipData.HexOffset(x, y);
		
		GameObject obj = Instantiate(ShipData.tileResource, new Vector3( hexpos.x, hexpos.y), Quaternion.identity ) as GameObject;
		obj.transform.parent = this.transform;
		HexaTile src = obj.GetComponent<HexaTile>();
			src.tileID = tileID;
			src.key = key;
			src.createDevice(tileID);
		
		tileSet.Add(key.index,src);
		if (tileID == 1) zero = src.key;	
		
		if (!check) return true;
		
		RecalcShip();
		
		if (src.connected) {
			RecalcEnergy();
			return true;
		}
		
		tileSet.Remove(src.key.index);
		Destroy(obj);
		
		RecalcShip();
		RecalcEnergy();
		
		return false;
	}
	
	public bool createTile(TilePoint tile, int tileID, bool check = false) {
		return createTile(tile.x, tile.y, tileID, check);
	}
	public bool createTile(Vector2 position, int tileID, bool check = false) {
		return createTile((int)position.x, (int)position.y, tileID, check);
	}
	
	public bool CreateDevice(string index, int device) {
		bool result = tileSet[index].createDevice(device);
		if (result) RecalcEnergy();
		return result;
	}
	
	public Vector3 zeroVector() {
		if (zero == null) return Vector3.zero;
		return zero.Vector();
	}

	public void RecalcShip() {
		foreach (HexaTile tile in tileSet.Values) {
			tile.getNeibors();
		}
		foreach (HexaTile tile in tileSet.Values) {
			tile.pathToZero();
		}
	}
	public void RecalcEnergy() {
		foreach (HexaTile tile in tileSet.Values) {
			tile.resetEnergy();
		}
		foreach (HexaTile tile in tileSet.Values) {
			tile.recalcEnergy();
		}
	}
			
	// Update is called once per frame
	void Update () {
		
	}
	
	public HexaTile GetTile(string index) {
		HexaTile tile = null;
		tileSet.TryGetValue(index, out tile);
		return tile;
	}
	
	public void SetSelected(string index, bool selected) {
		HexaTile tile = null;
		tileSet.TryGetValue(index, out tile);
		if (tile != null) tile.setSelected(selected);
	}
	
	public void DeleteTile(string index, bool destroy = false) {
		HexaTile tile = tileSet[index];
		
		
		tileSet.Remove(index);
    tile.Demolish(destroy, false);
		
		List<HexaTile> rec =new List<HexaTile>();
		foreach (HexaTile t in tileSet.Values) {
			t.Reconnect(index);
			rec.Add(t);
		}
			
		foreach (HexaTile t in rec) t.pathToZero();
		
		if (!destroy) {
			RecalcEnergy();
			return;
		}
		
		List<string> ind = new List<string>();
		foreach (HexaTile t in tileSet.Values) {
			if (!t.connected) ind.Add(t.key.index);
		}
		
		foreach (string idx in ind) {
			tileSet[idx].Demolish(true, true);
			tileSet.Remove(idx);
		}
		RecalcEnergy();
  }
  
	public bool DeleteDevice(string index) {
		HexaTile tile = tileSet[index];
		bool result = tile.deleteDevice();
		if (result) RecalcEnergy();
		
		return result;
  }
  
	
}

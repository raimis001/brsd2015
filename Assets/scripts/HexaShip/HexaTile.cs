using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexaTile : MonoBehaviour {

	[HideInInspector]
	public int tileID = 0;
	public TilePoint key;
	public bool connected = true;
	
	//public int energy = 0;
	//public int energyNeed = 0;

	public float hp = 1f;
	public float hpMax = 1f;
	
	
	public DeviceData device = null;

	public delegate void MouseClick(TilePoint key);
	public static event MouseClick OnMouseClick;

	// Use this for initialization
	void Start () {
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.sortingOrder = key.y;// -(int)transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void recalcEnergy() {
		if (device == null || key == null || key.AllNeighbours == null || key.Count < 1) return;
		
		if (device.energyProduce > 0) {
			int cnt = 0;
			foreach (TilePoint point in key.AllNeighbours) {
				HexaTile tile = key.ship.GetTile(point.index);
				if (tile != null && tile.device.energyNeed > 0) {
					cnt++;
					tile.device.energyCurrent = 0;
				}
			}
			if (cnt > 0) {
			int en = device.energyProduce / cnt;
				foreach (TilePoint point in key.AllNeighbours) {
					HexaTile tile = key.ship.GetTile(point.index);
					if (tile != null && tile.device.energyNeed > 0) tile.device.energyCurrent += en;
				}
			}
		} else {
			foreach (TilePoint point in key.AllNeighbours) {
				HexaTile tile = key.ship.GetTile(point.index);
				if (tile != null && tile.device.energyProduce > 0) tile.recalcEnergy(); 
			}
		}
		
		
	}
		
	public bool createDevice(int deviceID) {
		if (device != null && device.id != 0) return false;
		tileID = deviceID;

		int energy = device != null ? device.energyCurrent : 0;
		device = DeviceData.createDevice(tileID, transform);
		hp = hpMax = device.hp;
		device.energyCurrent = energy;
		
		recalcEnergy();
		
		ShipData.addEnergy(device.energy);
		
		Turret turret;
		switch (tileID) {
			case 1:
				key.ship.zero = key;
				break;
			case 2:
				turret = gameObject.AddComponent<Turret>();
				turret.data = device;
				break;
			case 6:
				turret = gameObject.AddComponent<Turret>();
				turret.data = device;
				break;
			case 11:
				ShipData.mainShip.tractor = key;
				break;
		}
		
		return true;
	}
	
	public bool isGenerator() {
		if (tileID == 1) return true;
		PathFinder.FindPath(key.ship.zero, key);
		return false;
	}
	
	public void getNeibors() {
		key.getNeibors();
	} 
	public void pathToZero() {
		connected = key.pathToZero();
		if (!connected) {
			show(3);
		} else {
			show(0);
		}
	}
	void OnMouseUp() {
		if (OnMouseClick != null) {
			OnMouseClick(key);
		}
	}
	public void show(int color) {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		switch (color) {
			case 0: //default
				sr.color = Color.white;
				break;
			case 1: //Selected
				sr.color = Color.blue;
				break;
			case 2: //Fitting
				sr.color = Color.yellow;
				break;
			case 3: //No connection
				sr.color = Color.red;
				break;
		}
	}
	 
	public void setSelected(bool selected) {
		show(selected ? 1 : 0);
		
		foreach (TilePoint point in key.AllNeighbours) {
			HexaTile tile = key.ship.GetTile(point.index);
			if (tile != null) tile.show(selected ? 2 : 0);
		}
		
		if (!selected) return;
		
		if (key.zeroPath == null) {
			show(3);
		} 
	}
	
	public void Demolish(bool explode, bool delay) {
		device = DeviceData.createDevice(0, transform);
		recalcEnergy();
			
		setSelected(false);
		if (explode) Explode.create(transform.position,delay);
    Destroy(gameObject);
	}
	
	public bool Reconnect(string index) {
		if (key.zeroPath == null) return false;
		foreach (TilePoint tile in key.AllNeighbours) {
			if (tile.index == index) {
				key.getNeibors();
				return true;
			}
		}
		return false;
	}
	public void ApplyDamage(float damage) {
		
		hp -= damage;
		if (hp <= 0 && key != null) {
			key.ship.DeleteTile(key.index,true);
			return;
		}
		
		int cnt = ShipData.tiles.Count - 1;
		int sprite = (int)(cnt - (hp / hpMax * cnt));
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.sprite = ShipData.tiles[sprite];
		//Debug.Log(sprite);
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		
		if (coll.gameObject.tag == "enemyShot") {
			//Debug.Log(coll.gameObject.name);
			Shot shot = coll.gameObject.GetComponent<Shot>();
			ApplyDamage(shot.damage);
			Destroy(coll.gameObject);
		}
		
	}
	
}

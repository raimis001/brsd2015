using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexaTile : MonoBehaviour {

	[HideInInspector]
	public int tileID = 0;
	public TilePoint key;
	public bool connected = true;
	
	public DeviceData device = null;

	public delegate void MouseClick(TilePoint key);
	public static event MouseClick OnMouseClick;

	// Use this for initialization
	void Start () {
		gameObject.name = "tile";
		
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.sortingOrder = key.y * 100;
		if (tag == "ship") {
			sr.sprite = ShipData.tiles[0];
		} else {
			sr.sprite = ShipData.tilesPirate[0];
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void resetEnergy() {
		if (device == null || device.energyNeed < 1) return;
		device.energyCurrent = 0;
	}
	
	public void recalcEnergy() {
		if (device == null || device.energyProduce < 1 || key == null || key.AllNeighbours == null || key.Count < 1) return;
			
		int cnt = 0;
		foreach (TilePoint point in key.AllNeighbours) {
			HexaTile tile = key.ship.GetTile(point.index);
			if (tile != null && tile.device.energyNeed > 0) {
				cnt++;
			}
		}
		if (cnt > 0) {
		int en = device.energyProduce / cnt;
			foreach (TilePoint point in key.AllNeighbours) {
				HexaTile tile = key.ship.GetTile(point.index);
				if (tile != null && tile.device.energyNeed > 0) tile.device.energyCurrent += en;
			}
		}
	}
		
	public bool createDevice(int deviceID) {
		if (device != null && device.id != 0) return false;
		tileID = deviceID;

		device = DeviceData.createDevice(tileID, transform);
		
		if (device.id > 0) {
			SpriteRenderer sr = device.gameObject.GetComponent<SpriteRenderer>();
			sr.sortingOrder = key.y * 100;
		}
		
		switch (tileID) {
			case 1:
				key.ship.zero = key;
				break;
		}
		
		return true;
	}
	public bool deleteDevice() {
		if (device == null || device.id == 0) return false;
		
		Destroy(device.gameObject);
		device = null;
		createDevice(0);
		return true;
	}
	
	public bool ugradeDevice(string param) {
		if (device == null || device.id == 0) return false;
		
		return device.upgrade(param);
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
				sr.color = new Color(237f / 255f,1f,69f / 255f);
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
		
		//foreach (TilePoint point in key.AllNeighbours) {
		//	HexaTile tile = key.ship.GetTile(point.index);
		//	if (tile != null) tile.show(selected ? 2 : 0);
		//}
		
		if (!selected) return;
		
		if (key.zeroPath == null) {
			show(3);
		} 
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
	
	public void Demolish(bool explode, bool delay) {
	
		setSelected(false);
		if (explode) Explode.create(transform.position,delay);
    Destroy(gameObject);
	}
	
	
	public void ApplyDamage(float damage) {
		
		device.hpCurrent -= (int)damage;
		if (device.hpCurrent <= 0 && key != null) {
			key.ship.DeleteTile(key.index,true);
			return;
		}
		
		int cnt = ShipData.tiles.Count - 1;
		int sprite = cnt - (int)((float)cnt * ((float)device.hpCurrent / (float)device.hpMax ));
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		Debug.Log("Set sprite " + sprite.ToString());
		if (tag == "ship") {
			sr.sprite = ShipData.tiles[sprite];
		} else {
			sr.sprite = ShipData.tilesPirate[sprite];
		}
	}
	
	public virtual void doShot(Shot collider) {
		ApplyDamage(collider.damage);
		Destroy(collider.gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.name ==  "rock") {
			float damage = coll.gameObject.GetComponent<Enemy>().damage;
			ApplyDamage(damage);
			Destroy(coll.gameObject);
			return;
		}
		if (coll.gameObject.name == "shot") {
			if (tag == "ship" && coll.gameObject.tag == "enemyShot") {
				float damage = coll.gameObject.GetComponent<Shot>().damage;
				ApplyDamage(damage);
				Destroy(coll.gameObject);
			}
			if (tag == "enemy" && coll.gameObject.tag == "shot") {
				float damage = coll.gameObject.GetComponent<Shot>().damage;
				ApplyDamage(damage);
				Destroy(coll.gameObject);
			}
			return;
		}
	}
	
	public string valueByName(string value) {
		string result = "";
		Debug.Log("get value for " + value);
		switch (value) {
			case "damage":
				result = device.damage.ToString();
				break;
			case "time":
				result = device.time.ToString("0.00");
				break;
			case "distance":
				result = device.distance.ToString("0");
				break;
			case "rate":
				result = device.rate.ToString("0.00");
				break;
			case "energy":
				result = device.energyProduce.ToString();
				break;
			case "speed":
				result = device.speed.ToString("0.00");
				break;
		}
		return result;
	}
}

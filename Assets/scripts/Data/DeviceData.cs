using UnityEngine;
using System.Collections;
using SimpleJSON;

public class DeviceData {
	
	public int id;
	public Sprite sprite;
	public string name;
	
	public int price;
	public int damage;
	public int hp;
	public float time;
	public float speed;
	public float distance;

	public int energy;
	public int energyNeed;
	public int energyProduce;
	public int energyCurrent = 0;
	
	public DeviceData(int id, JSONNode node, Sprite sprite) {
		
		this.id = id;
		this.sprite = sprite;
		
		this.name = node["name"];
		this.price = node["price"].AsInt;
		this.hp = node["hp"].AsInt;
		this.damage = node["damage"].AsInt;
		this.speed = node["speed"].AsFloat;
		this.time = node["time"].AsFloat;
		this.distance = node["distance"].AsFloat;
		this.energy = node["energy"].AsInt;
			
	}	
	
	public DeviceData(DeviceData data) {
		this.id = data.id;
		//this.sprite = data.sprite;
		
		this.name = data.name;
		this.price = data.price;
		this.hp = data.hp;
		this.damage = data.damage;
		this.speed = data.speed;
		this.time = data.time;
		this.distance = data.distance;
		this.energy = data.energy;

		if (this.energy < 0)
			this.energyNeed = -this.energy;
			else this.energyProduce = this.energy;
						
	}
	
	public static DeviceData createDevice(int id, Transform parent) {
		DeviceData data = ShipData.devices[id];
		if (data.id == 0) return new DeviceData(data);
			
		GameObject obj;
	
		if (id != 4) {
			obj = new GameObject("device");
			
			SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
				sr.sprite = data.sprite;
				sr.sortingLayerName = "devices";
				obj.transform.position = parent.position;
		} else {
			obj = HexaShip.Instantiate(Resources.Load("shield"), parent.position, Quaternion.identity) as GameObject; 
		}
		
		obj.transform.parent = parent;
		
		data = new DeviceData(data);
		switch (id) {
		case 2:
			obj.AddComponent<Turret>().data = data;
			break;
		case 4:
			obj.AddComponent<Shield>().data = data;
			break;
		case 6:
			obj.AddComponent<Turret>().data = data;
			break;
		case 7:
			obj.AddComponent<Fabric>().data = data;
			break;
		case 8:
			obj.AddComponent<Laboratory>().data = data;
			break;
		}
		
		return data;
	}
	
}

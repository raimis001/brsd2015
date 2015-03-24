using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class DeviceData {
	
	public int id;
	public Sprite sprite;
	public string name;
	
	public int price;
	public int damage;
	public float rate;
	public float time;
	public float speed;
	public float distance;

	public int hp;
	public int hpCurrent;
	public int hpMax;

	public int energy;
	public int energyNeed;
	public int energyProduce;
	public int energyCurrent = 0;
	
	public int level = 1;
	
	public Dictionary<string,UpgradeData> upgrades = new Dictionary<string, UpgradeData>();
	
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
		this.rate = node["rate"].AsFloat;

		if (node["upgrade"] != null) {
			for (int i = 0; i < node["upgrade"].Count; i++) {
				string n = node["upgrade"].AsObject.keyAt(i);
				upgrades.Add(n, new UpgradeData(
					node["upgrade"][i]["price"].AsInt,
					node["upgrade"][i]["value"].AsInt,
					node["upgrade"][i]["energy"].AsInt
					));
			}
		}
		
		hpCurrent = hpMax = hp;
						
	}	
	public bool isEnergy() {
		return energyCurrent >= energyNeed;
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
		this.rate = data.rate;
		this.upgrades = data.upgrades;
		
		if (this.energy < 0)
			this.energyNeed = -this.energy;
			else this.energyProduce = this.energy;

		hpCurrent = hpMax = hp;
	}
	
	public GameObject gameObject;
	
	public static DeviceData createDevice(int id, Transform parent) {
		DeviceData data = ShipData.devices[id];
		if (data.id == 0) return new DeviceData(data);
		Sprite sprite = data.sprite;
		
		data = new DeviceData(data);
		
		if (id != 4) {
			data.gameObject = new GameObject("device");
			
			SpriteRenderer sr = data.gameObject.AddComponent<SpriteRenderer>();
				sr.sprite = sprite;
				sr.sortingLayerName = "devices";
			data.gameObject.transform.position = parent.position;
		} else {
			data.gameObject = HexaShip.Instantiate(Resources.Load("shield"), parent.position, Quaternion.identity) as GameObject; 
			
		}
		
		data.gameObject.transform.parent = parent;
		
		switch (id) {
			case 2:
				data.gameObject.AddComponent<Turret>().data = data;
				break;
			case 4:
				data.gameObject.GetComponent<Shield>().data = data;
				break;
			case 6:
				data.gameObject.AddComponent<Turret>().data = data;
				break;
			case 7:
				data.gameObject.AddComponent<Fabric>().data = data;
				break;
			case 8:
				data.gameObject.AddComponent<Laboratory>().data = data;
				break;
			case 11:
				data.gameObject.AddComponent<Tractor>().data = data;
				break;
		}
		
		return data;
	}
	
	public bool upgrade(string param) {
		if (!upgrades.ContainsKey(param)) return false;
		
		UpgradeData data = upgrades[param];
		
		if (ShipData.knowledge < data.price) return false;
		if (data.value == 0) return false;
			
		ShipData.addKnowledge(-data.price);
		data.level ++;
		    
		if (data.energy != 0) 
			energyNeed = (int)((float)energyNeed * (1f + (1f / (float)data.energy)));
		    
		    
		switch (param) {
			case "damage":
			 	damage = (int)((float)damage * (1f + (1f / (float)data.value)));
				break;
			case "time":
				time = ((float)time * (1f + (1f / (float)data.value)));
				break;
			case "distance":
				distance = ((float)distance * (1f + (1f / (float)data.value)));
				break;
			case "speed":
				speed = ((float)speed * (1f + (1f / (float)data.value)));
				break;
			case "rate":
				rate = ((float)rate * (1f + (1f / (float)data.value)));
				break;
		}
		
		return true;
		
	}
	
}

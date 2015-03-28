using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class DeviceData {
	
	public int id;
	public string name;
	public string prefab;
	
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
	
	public GameObject gameObject;
	
	public Dictionary<string,UpgradeData> upgrades = new Dictionary<string, UpgradeData>();
	
	public DeviceData(int id, JSONNode node) {
		
		this.id = id;
		this.prefab = node["atlas"];
		
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
		this.prefab = data.prefab;
		
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
	
	public void UpdateData(DeviceData data) {

		if (data.price > 0) this.price = data.price;
		if (data.hp > 0) this.hp = data.hp;
		if (data.damage > 0) this.damage = data.damage;
		if (data.speed > 0) this.speed = data.speed;
		if (data.time > 0) this.time = data.time;
		if (data.distance > 0) this.distance = data.distance;
		if (data.energy != 0) this.energy = data.energy;
		if (data.rate > 0) this.rate = data.rate;
		
		if (this.energy < 0)
			this.energyNeed = -this.energy;
			else this.energyProduce = this.energy;
			
		hpCurrent = hpMax = hp;
	}
	
	public static DeviceData createDevice(int id, Transform parent) {
		DeviceData data = ShipData.devices[id];
		if (data.id == 0) return new DeviceData(data);
		GameObject prefab = Resources.Load("devices/" + data.prefab) as GameObject;
		if (prefab == null) return null;
			
		data = new DeviceData(data);
			data.gameObject = HexaShip.Instantiate(prefab, parent.position, Quaternion.identity) as GameObject; 
			data.gameObject.transform.parent = parent;
			data.gameObject.tag = parent.gameObject.tag;
		
		Device device = data.gameObject.GetComponent<Device>();
		if (device != null) device.data = data;
		
		return data;
	}
	
	public bool upgrade(string param) {
		if (!upgrades.ContainsKey(param)) return false;
		
		UpgradeData data = upgrades[param];
		
		if (ShipData.knowledge < data.price) return false;
			
		DeviceData mainDevice = ShipData.devices[id];
		ShipData.addKnowledge(-data.price);
		data.level ++;
		    
		energyNeed = (int)((float)mainDevice.energyNeed * ((float)data.energy / 100f));
		    
		switch (param) {
			case "damage":
				damage = (int)(damage + (float)mainDevice.damage * ((float)data.value/ 100f));
				break;
			case "time":
				time += ((float)mainDevice.time * ((float)data.value/ 100f));
				break;
			case "distance":
				distance += ((float)mainDevice.distance * ((float)data.value/ 100f));
				break;
			case "speed":
				speed += ((float)mainDevice.speed * ((float)data.value/ 100f));
				break;
			case "rate":
				rate += ((float)mainDevice.rate * ((float)data.value/ 100f));
				break;
			case "energy":
				energy = (int)(energy + (float)mainDevice.energy * ((float)data.value/ 100f));
				if (this.energy < 0)
					this.energyNeed = -this.energy;
					else this.energyProduce = this.energy;
			
				break;
		}
		
		return true;
		
	}
	
}

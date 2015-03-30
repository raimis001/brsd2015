using UnityEngine;
using System.Collections;
using SimpleJSON;

public class UpgradeData {
	
	public int value;
	public int energy;
	public int level = 1;
	public string label;
	
	private int[] prices = new int[3];
	public int price {
		get { return prices[level-1]; }
	}
	
	public UpgradeData(JSONNode node) {
			
		JSONArray arr = node["price"].AsArray;
		for (int i = 0; i < arr.Count; i++) {
			prices[i] = arr[i].AsInt;
		}
			
		//this.prices = node["price"].AsArray;
		this.label = node["name"];
		this.value = node["value"].AsInt;
		this.energy = node["energy"].AsInt;
	}
	public UpgradeData(UpgradeData data) {
		this.label = data.label;
		this.value = data.value;
		this.energy = data.energy;
		
		
		for (int i = 0; i < 3; i++) {
			prices[i] = data.prices[i];
		}
	}
		
}

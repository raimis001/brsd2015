using UnityEngine;
using System.Collections;
using SimpleJSON;

public class SpawnData {

	public float angle = 0f;
	public int count = 1;
	public int spread = 10;
	public float speed = 1f;
	public float damage = 1f;
	public float hp = 1f;
	public float delay = 0f;
	public float distance;
	public int value = 0;
	public float charge = 0;
	
	//"count":10, "angle":90, x:100,y:100,"spread":10},	
	public SpawnData (JSONNode node) {
		
		angle = Mathf.Deg2Rad * node["angle"].AsFloat;
		count = node["count"].AsInt;
		spread = node["spread"].AsInt;
		speed = node["speed"].AsFloat > 0 ? node["speed"].AsFloat : 1f;
		damage = node["damage"].AsFloat > 0 ? node["damage"].AsFloat : 1f;
		hp = node["hp"].AsFloat > 0 ? node["hp"].AsFloat : 10f;
		delay = node["delay"].AsFloat;
		distance = node["distance"].AsFloat > 0 ? node["distance"].AsFloat : 10f;
		value = node["value"].AsInt;
		charge = node["charge"].AsFloat;
	}	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EnemyData 	{

	public Dictionary<TilePoint, DeviceData> ship = new Dictionary<TilePoint, DeviceData>();

	public EnemyData (JSONNode json) {
		for (int i = 0; i < json.Count; i++) {
			ship.Add(new TilePoint(json[i]["x"].AsInt,json[i]["y"].AsInt),new DeviceData(json[i]["device"]["id"].AsInt, json[i]["device"]));
		}
		
	}
}



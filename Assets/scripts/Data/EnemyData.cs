using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EnemyData 	{

	public Dictionary<TilePoint, int> ship = new Dictionary<TilePoint, int>();

	public EnemyData (JSONNode json) {
		for (int i = 0; i < json.Count; i++) {
			ship.Add(new TilePoint(json[i]["x"].AsInt,json[i]["y"].AsInt),json[i]["device"]["id"].AsInt);
		}
		
	}
}



using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;


public class EventData {

	
	public Dictionary<string, SpawnData> spawners = new Dictionary<string, SpawnData>();
	
	public bool dispatched = false;
	
	public EventData(JSONNode node) {
		
		for (int i = 0; i < node.Count; i++) {
			spawners.Add(node.AsObject.keyAt(i), new SpawnData(node[i]));
		}
		
	}
	
	public void Dispatch() {
		if (dispatched) return;
		
		dispatched = false;
	
		foreach (KeyValuePair<string, SpawnData> key in spawners) {
			//Debug.Log(key.Key +  ":" + key.Value.count);
			Enemy.create(key.Key, key.Value);
		}
				
	}

}

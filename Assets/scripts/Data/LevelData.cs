using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class LevelData {

	public float time = 0f;
	public float currentTime = 0f;
	public string name;
	
	public Dictionary<int, EventData> events = new Dictionary<int, EventData>();
	
	public LevelData(JSONNode node) {
		time = node["time"].AsFloat;
		currentTime = time;
		name = node["baseName"];
		
		for (int i = 0; i < node["events"].Count; i++) {
			events.Add(int.Parse(node["events"].AsObject.keyAt(i)), new EventData(node["events"][i]));
		}
	}	

}

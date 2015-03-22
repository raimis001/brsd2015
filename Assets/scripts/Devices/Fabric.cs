using UnityEngine;
using System.Collections;

public class Fabric : Device {

	float timer = 0;

	// Use this for initialization
	void Start () {
		Debug.Log("Start fabric");
		
		timer = data.time;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (data.energyCurrent < data.energyNeed || ShipData.scraps < ShipData.metalPrice) {
			timer = data.time;
			return;
		}
		
		if (timer > 0) {
			timer -= Time.smoothDeltaTime;
			if (timer <= 0) {
				timer = data.time;
				ShipData.addScraps(-ShipData.metalPrice);
				ShipData.addMetals(1);
			}
		}
		
	}
}

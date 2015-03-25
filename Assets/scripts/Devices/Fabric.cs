using UnityEngine;
using System.Collections;

//https://github.com/raimis001/brsd2015/wiki/Fabric
public class Fabric : Device {
	
	float timer = 1;
	float rate = 1;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		if (data != null) {
			timer = data.time;
			rate = data.rate;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if (Gui.gameMode != 1 || data.energyCurrent < data.energyNeed || ShipData.scraps < rate) {
			timer = data.time;
			return;
		}
		
		if (timer > 0) {
			timer -= Time.smoothDeltaTime;
			if (timer <= 0) {
				timer = data.time;
				ShipData.addScraps(-(int)rate);
				ShipData.addMetals(1);
			}
		}
		
	}
}

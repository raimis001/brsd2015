using UnityEngine;
using System.Collections;

public class Laboratory : Device {

	float timer;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		timer = data.time;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if (Gui.gameMode != 1 || data.energyCurrent < data.energyNeed) {
			timer = data.time;
			return;
		}
		
		if (timer > 0) {
			timer -= Time.smoothDeltaTime;
			if (timer <= 0) {
				timer = data.time;
				ShipData.addKnowledge(1);
			}
		}
		
		
	}
}

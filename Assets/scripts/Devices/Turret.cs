using UnityEngine;
using System.Collections;

public class Turret : Device {

	float shot = 0;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();	
		shot = data.time;	
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		
		if (data == null || data.energyCurrent < data.energyNeed) return;
		
		if (shot > 0) {
			shot -= Time.smoothDeltaTime;
			return;
		}
		
		shot = data.time;
		GameObject enemy = FindClosestEnemy(type, data.distance);
		if (enemy == null) return;
		Vector3 pos = transform.position;
		pos.y += 0.8f;
		
		Vector3 dir = enemy.transform.position - pos;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
		Shot.create(pos, angle, data, shotType);
	}
	
}

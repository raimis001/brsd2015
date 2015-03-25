using UnityEngine;
using System.Collections;

public class Rocket : Device {

	float shot = 0;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if (data.energyCurrent < data.energyNeed) return;
		
		if (shot > 0) {
			shot -= Time.smoothDeltaTime;
			return;
		}
		
		shot = data.time;
		GameObject enemy = FindClosestEnemy(type, data.distance);
		if (enemy == null) return;
		
		Vector3 dir = enemy.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
		
		Shot.create(transform.position, angle, data, shotType);
	}
}

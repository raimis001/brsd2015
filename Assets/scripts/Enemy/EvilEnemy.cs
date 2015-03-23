using UnityEngine;
using System.Collections;

public class EvilEnemy : Enemy {

	GameObject target = null;
	float range = 5f;
	float velocity = 0.2f;
	float charge = 1f;
	float maxCharge = 3f;

	// Use this for initialization
	override protected void Start () {
		base.Start();
		if (data != null) {
			range = data.range;
			charge = data.charge;
			velocity = data.velocity;
			maxCharge = data.charge;
		} 
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
	
		if (target == null || target.gameObject == null) {
			target = FindClosestEnemy("ship",30f);
		}
		
		if (target != null) {
			float distance = Vector2.Distance(transform.position, target.transform.position);
			if (distance > range) {
				transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
			} else {
				float rot = speed / 10f;
				transform.RotateAround(target.transform.position, new Vector3(0,0,1f), rot);
				transform.Rotate(new Vector3(0,0,1f), -rot);
				
				if (charge <= 0) {
					Vector3 dir = target.transform.position - transform.position;
					float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
					Shot.create(transform.position,angle,velocity, (int)damage, "enemyShot", "Shot");
					charge = maxCharge;
				} else {
					charge -= Time.deltaTime;
				}
								
				target = FindClosestEnemy("ship",30f);
			}
		
		}
	}
}

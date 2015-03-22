using UnityEngine;
using System.Collections;

public class EvilEnemy : Enemy {

	GameObject target = null;
	float range = 2f;
	float charge = 0;
	float velocity = 2f;

	// Use this for initialization
	override protected void Start () {
		base.Start();
		range = data.range;
		charge = data.charge;
		velocity = data.velocity;
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
		//if (!GetComponent<SpriteRenderer>().enabled) return;
	
		if (target == null) {
			GameObject obj = FindClosestEnemy("ship",30f);
			if (obj != null) {
				Debug.Log("Found ship");
				target = obj;
			}
		}
		
		if (target != null) {
			if (target.name == "dummyTarget") {
				transform.position = Vector3.RotateTowards(transform.position,target.transform.position ,0.002f, speed * Time.deltaTime);
				GameObject obj = FindClosestEnemy("ship",30f);
				if (obj == null) return;
				if (Vector2.Distance(transform.position, obj.transform.position) > range * 1.5f) {
					Destroy(target);
					target = obj;
				}
				return;
			}
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
			if (Vector2.Distance(transform.position, target.transform.position) < range) {
				
				if (charge > 0) {
					charge -= Time.deltaTime;
					if (charge <= 0) {
						Vector3 dir = target.transform.position - transform.position;
						float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
						
						Shot.create(transform.position,angle,velocity, (int)damage, "enemyShot", "Shot");
						charge = data.charge;
					}
				}
			}
			
			if (Vector2.Distance(transform.position, target.transform.position) < 3f) {
				
				Vector3 dir = target.transform.position - transform.position;
				float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg + 45;
				
				
				target = new GameObject("dummyTarget");
				target.transform.position = target.transform.position;
				target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				target.transform.Translate(new Vector3(25f, 0));
			}
					
		} else {
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);		
		}
		
	}
}

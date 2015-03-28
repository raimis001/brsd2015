using UnityEngine;
using System.Collections;

public class Shield : Device {


	float scale = 1f;
	Vector3 localScale;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
		gameObject.name = "shield";
		localScale = transform.localScale;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if (scale < 1f) {
			if (data.isEnergy()) {
				scale += (data.rate / 1000);
				if (scale >= 1f) scale = 1f;
			}
			transform.localScale = scale * localScale;
		}
	}
	
	public void ApplyDamage(float damage) {
		if (scale > 0) {
			scale -= (damage / data.damage) * 0.1f;
			if (scale < 0) scale = 0;
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		
		if (coll.gameObject.name == "rock") {
			float damage = coll.gameObject.GetComponent<Enemy>().damage;
			ApplyDamage(damage);
			Destroy(coll.gameObject);
			return;
		}
		if (coll.gameObject.name == "shot") {
			if (tag == "ship" && coll.gameObject.tag == "enemyShot") {
				float damage = coll.gameObject.GetComponent<Shot>().damage;
				ApplyDamage(damage);
				Destroy(coll.gameObject);
			}
			if (tag == "enemy" && coll.gameObject.tag == "shot") {
				float damage = coll.gameObject.GetComponent<Shot>().damage;
				ApplyDamage(damage);
				Destroy(coll.gameObject);
			}
			return;
		}
	}
	
}

using UnityEngine;
using System.Collections;

public class Shield : Device {


	float scale = 1f;
	Vector3 localScale;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
		gameObject.name = "shield";
		localScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (scale < 1f) {
			if (data.isEnergy()) {
				scale += (data.rate / 1000);
				Debug.Log("restore shield:" + data.rate.ToString());
				if (scale >= 1f) scale = 1f;
			}
			transform.localScale = scale * localScale;
		}
	}
	
	public void ApplyDamage(float damage) {
		if (scale > 0) scale -= damage / data.damage;
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

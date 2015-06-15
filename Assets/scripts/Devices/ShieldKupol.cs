using UnityEngine;
using System.Collections;

public class ShieldKupol : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		
		if (coll.gameObject.name == "rock") {
			float damage = coll.gameObject.GetComponent<Enemy>().damage;
			transform.parent.gameObject.SendMessage("ApplyDamage",damage);
			Destroy(coll.gameObject);
			return;
		}
		if (coll.gameObject.name == "shot") {
			if (tag == "ship" && coll.gameObject.tag == "enemyShot") {
				float damage = coll.gameObject.GetComponent<Shot>().damage;
				transform.parent.gameObject.SendMessage("ApplyDamage",damage);
				Destroy(coll.gameObject);
			}
			if (tag == "enemy" && coll.gameObject.tag == "shot") {
				float damage = coll.gameObject.GetComponent<Shot>().damage;
				transform.parent.gameObject.SendMessage("ApplyDamage",damage);
				Destroy(coll.gameObject);
			}
			return;
		}
	}
	
}

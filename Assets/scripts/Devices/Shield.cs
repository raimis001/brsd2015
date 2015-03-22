using UnityEngine;
using System.Collections;

public class Shield : Device {


	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
		gameObject.name = "shield";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ApplyDamage(float damage) {
	}
	public override void doShot (Shot collider) {
		//base.doShot (collider);
		if (collider.gameObject.tag != "enemyShot") return;
		Destroy(collider.gameObject);
	}
}

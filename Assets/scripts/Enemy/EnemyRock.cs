using UnityEngine;
using System.Collections;

public class EnemyRock : Enemy {

	Vector3 _rotate;

	// Use this for initialization
	override protected void Start () {
		base.Start();
		_rotate = new Vector3(0,0, Random.Range(-2f, 2f));
		name = "rock";
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
		
		if (GetComponent<SpriteRenderer>().enabled) {
			transform.Rotate(_rotate);
			transform.position = Vector3.MoveTowards(transform.position, ShipData.mainShip.zeroVector(), speed * Time.deltaTime);		
		} 
		
	}
}

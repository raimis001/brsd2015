using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Medusa : Enemy {


	float charge;
	Animator animator;

	// Use this for initialization
	override protected void Start () {
		base.Start();
		charge = data.charge;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
		if (!GetComponent<SpriteRenderer>().enabled) return;
		
		if (charge > 0) {
			charge -= Time.deltaTime;
			return;
		}
		

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.55f);
		List<HexaTile> tiles = new List<HexaTile>();
		foreach (Collider2D collider in colliders) {
			if (collider.gameObject.tag != "ship") continue;
			tiles.Add(collider.gameObject.GetComponent<HexaTile>());
			//Debug.Log(collider.gameObject.tag);
		}
					
		if (tiles.Count > 0) {
			charge = data.charge;
			if (animator != null) animator.SetTrigger("atack");
			foreach (HexaTile tile in tiles) tile.ApplyDamage(data.damage);
			return;
		}
		if (animator != null) animator.SetTrigger("go");
		
		if (GetComponent<SpriteRenderer>().enabled) {
			transform.position = Vector3.MoveTowards(transform.position, ShipData.mainShip.zeroVector(), speed * Time.deltaTime);		
		}
		
	}
}

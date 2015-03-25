using UnityEngine;
using System.Collections;

//https://github.com/raimis001/brsd2015/wiki/Tractor-beam
public class Tractor : Device {

	Scrap scrap = null;
	float modifier = 1f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if (!data.isEnergy()) return;
		
		if (scrap == null) {
			GameObject obj = FindClosestScrap("scraps",data.distance);
			if (obj != null) scrap = obj.GetComponent<Scrap>();
		}
		
		if (scrap != null) {
			scrap.Beam(transform.position, data.speed * modifier);
			modifier = Mathf.Lerp(modifier, 3f, 0.01f);
			if (Vector3.Distance(transform.position, scrap.transform.position) < 0.05f) {
				ShipData.addScraps(scrap.value);
				Destroy(scrap.gameObject);
				scrap = null;
			}
		} 
			
		
		
	}
	
	GameObject FindClosestScrap(string tag, float radius) {
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		float border = radius * radius;
		
		foreach (GameObject go in gos) {
			Scrap scrap = go.GetComponent<Scrap>();
			if (scrap == null || scrap.beamed) continue;
			
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < border && curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
	
}

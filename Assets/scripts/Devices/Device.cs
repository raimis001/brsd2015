using UnityEngine;
using System.Collections;

public class Device : MonoBehaviour {

	public DeviceData data;
	protected string type;
	protected string shotType;
	
	// Use this for initialization
	protected virtual void Start () {
		if (gameObject.tag == "enemy") {
			type = "ship";
			shotType = "enemyShot";
		} else {
			type = "enemy";
			shotType = "shot";
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	}
	
	protected GameObject FindClosestEnemy(string tag, float radius) {
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		float border = radius * radius;
		
		foreach (GameObject go in gos) {
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

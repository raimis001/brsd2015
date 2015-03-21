using UnityEngine;
using System.Collections;

public class Device : MonoBehaviour {

	public DeviceData data;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

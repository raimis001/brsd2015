using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	
	public static void create(Vector3 position, float direction, DeviceData data, string tag) {
		string prefab = null;
		switch (data.id) {
			case 2:
				prefab = "Shot";
				break;
			case 6:
				prefab = "Rocket";
				break;
			
		}
		if (prefab == null) return;		
		
		Shot.create(position, direction, data.speed, data.damage, tag, prefab);
	}
	
	public static void create(Vector3 position, float direction, float speed, int damage, string tag , string prefab) {
	
		Quaternion angle = Quaternion.AngleAxis(direction, Vector3.forward);
		Shot shot = (Instantiate (Resources.Load (prefab),position,angle)as GameObject).GetComponent<Shot>();
			
		shot.speed = speed;
		shot.damage = damage;
		shot.gameObject.tag = tag;
	
		Debug.Log("Creating shot with tag:" + tag);	
	}

	public float speed;	
	public int damage;
	

	Vector3 direction;
	Vector3 start;
	
	// Use this for initialization
	void Start () {
		start = transform.position;
		direction = new Vector3(0.2f,0f,0) * speed;
		name = "shot";
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(direction);
		
		if (Vector3.Distance(start, transform.position) > 20f) {
			Destroy(gameObject);
			return;
		}
	}

}

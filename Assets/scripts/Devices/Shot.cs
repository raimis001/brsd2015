using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	
	public static void create(Vector3 position, float direction, DeviceData data) {
		Quaternion angle = Quaternion.AngleAxis(direction, Vector3.forward);
		Shot shot = null;
		switch (data.id) {
			case 2:
				shot = (Instantiate (Resources.Load ("Shot"),position,angle)as GameObject).GetComponent<Shot>();
				break;
			case 6:
				shot = (Instantiate (Resources.Load ("Rocket"),position,angle)as GameObject).GetComponent<Shot>();
				break;
			
		}
		if (shot == null) return;		
		
		shot.speed = data.speed;
		shot.damage = data.damage;
	}
	
	public static void create(Vector3 position, float direction, float speed, int damage, string tag , string prefab) {
	
		Quaternion angle = Quaternion.AngleAxis(direction, Vector3.forward);
		Shot shot = (Instantiate (Resources.Load (prefab),position,angle)as GameObject).GetComponent<Shot>();
			
		shot.gameObject.tag = tag;
		shot.speed = speed;
		shot.damage = damage;
		
	}

	public float speed;	
	public int damage;
	

	Vector3 direction;
	Vector3 start;
	
	// Use this for initialization
	void Start () {
		start = transform.position;
		direction = new Vector3(0.2f,0f,0) * speed;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(direction);
		
		if (Vector3.Distance(start, transform.position) > 20f) {
			Destroy(gameObject);
			return;
		}
		
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.9f);
		
		foreach (Collider2D coll in colliders) {
			if (coll.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()) continue;
			if (coll.gameObject.name == "shield") {
				if (gameObject.tag == "enemyShot") {
					if (coll.gameObject.tag == "ship") {
						Device device = coll.gameObject.GetComponent<Device>();						
						if (device) device.doShot(this);
						return;
					}
				} else {
					if (coll.gameObject.tag == "enemy") {
						Device device = coll.gameObject.GetComponent<Device>();						
						if (device) device.doShot(this);
						return;
					}
				}
			}
		}
		
		foreach (Collider2D coll in colliders) {
			if (coll.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()) continue;
			if (coll.gameObject.name == "tile") {
				if (gameObject.tag == "enemyShot") {
					if (coll.gameObject.tag == "ship") {
						HexaTile tile = coll.gameObject.GetComponent<HexaTile>();
						if (tile != null) tile.doShot(this);
						return;
					}
				} else {
					if (coll.gameObject.tag == "enemy") {
						HexaTile tile = coll.gameObject.GetComponent<HexaTile>();
						if (tile != null) tile.doShot(this);
						return;
					}
				}
			}
		}
		
	}
}

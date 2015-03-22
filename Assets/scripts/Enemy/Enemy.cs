using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float damage = 1f;
	public float hp = 10f;
	public float delay = 0f;
	public int scraps = 0;
	public float speed = 0.1f;
	
	public SpawnData data;
	
	public static void create(string type, SpawnData data) {
		
		GameObject prefab = Resources.Load(type) as GameObject;
		if (prefab == null) return;	
			
		float delay = 0f;
		for (int i = 0; i < data.count; i++) {
			float angle = data.angle + (Random.Range(-10f,10f) * Mathf.Deg2Rad);
			Vector3 pos = new Vector3(Mathf.Sin(angle),Mathf.Cos(angle), 0) * data.distance ;
			Enemy enemy = (Instantiate(prefab, pos, Quaternion.identity) as GameObject).GetComponent<Enemy>();
			enemy.delay = delay;
			enemy.data = data;
				
			delay += data.delay;
		}
		
	}

	// Use this for initialization
	protected virtual void Start () {
		speed = data.speed * Random.Range(0.9f, 1.2f);
		hp = data.hp;
		damage = data.damage;
		scraps = data.value;
		
		if (delay > 0) {
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (delay > 0) {
			delay -= Time.smoothDeltaTime;
			if (delay <= 0) {
				delay = 0;
				GetComponent<SpriteRenderer>().enabled = true;
			}
		}
	}
	
	void explode() {
		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
	
		if (coll.gameObject.tag == "ship") {
				collideShip(coll.gameObject);
				return;
		}
		
		if (coll.gameObject.tag == "shot") {
			if (coll.gameObject != null) {
				collideShot(coll.gameObject.GetComponent<Shot>());
				Destroy(coll.gameObject);
			}
		}
		
	}
	protected virtual void collideShip(GameObject collider) {
		if (collider != null) {
			collider.SendMessage("ApplyDamage", damage);
			explode();
		}
		
	}
	protected virtual void collideShot(Shot shot) {
		if (shot == null) return;
		
		hp -= shot.damage;
		//Debug.Log(hp + " : " + shot.damage);
		if (hp <= 0) {
			ShipData.addScraps(scraps);
			explode();
		}
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

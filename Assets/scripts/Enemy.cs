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
		
		string prefab = null;
		if (ShipData.rocks.ContainsKey(type)) {
			prefab = "Rock";
		} else if (type.Equals("medusa")) {
			prefab = "Medusa";
		}
		
		if (prefab == null) return;	
				
			
		float delay = 0f;
		for (int i = 0; i < data.count; i++) {
			float angle = data.angle + (Random.Range(-10f,10f) * Mathf.Deg2Rad);
			Vector3 pos = new Vector3(Mathf.Sin(angle),Mathf.Cos(angle), 0) * data.distance ;
			Enemy enemy = (Instantiate(Resources.Load(prefab), pos, Quaternion.identity) as GameObject).GetComponent<Enemy>();
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
			if (coll.gameObject != null) {
				coll.gameObject.SendMessage("ApplyDamage", damage);
			}
			explode();
			return;
		}
		
		if (coll.gameObject.tag == "shot") {
			Shot shot = coll.gameObject.GetComponent<Shot>();
			hp -= shot.damage;
			Destroy(coll.gameObject);
			if (hp <= 0) {
				ShipData.addScraps(scraps);
				explode();
			}
		}
		
	}
	
}

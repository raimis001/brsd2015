using UnityEngine;
using System.Collections;

public class Scrap : MonoBehaviour {


	public static void create(Vector3 position, int value) {
		
		position = new Vector3(position.x * (1 - Random.Range(-0.1f, 0.1f)),position.y * (1 - Random.Range(-0.1f, 0.1f)));
		Scrap scrap = (Instantiate(Resources.Load("Scrap"), position, Quaternion.identity) as GameObject).GetComponent<Scrap>();
			scrap.value = value;
	}

	public int value;
	Vector3 _rotate;
	float _speed = 2f;

	// Use this for initialization
	void Start () {
		_rotate = new Vector3(0,0, Random.Range(-2f, 2f));
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(_rotate);
		if (ShipData.mainShip.tractor == null || !ShipData.mainShip.tractor.Working()) {
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, -_speed * Time.deltaTime * 0.2f);	
			if (Vector3.Distance(transform.position, Vector3.zero) > 20f) {
				Destroy(gameObject);
			}	
		} else {
			transform.position = Vector3.MoveTowards(transform.position, ShipData.mainShip.tractor.Vector(), _speed * Time.deltaTime);		
			if (Vector3.Distance(transform.position, ShipData.mainShip.tractor.Vector()) < 0.05f) {
				ShipData.addScraps(value);
				Destroy(gameObject);
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class Scrap : MonoBehaviour {


	public static void create(Vector3 position, int value) {
		position = new Vector3(position.x * (1 - Random.Range(-0.1f, 0.1f)),position.y * (1 - Random.Range(-0.1f, 0.1f)));
		Scrap scrap = (Instantiate(Resources.Load("Scrap"), position, Quaternion.identity) as GameObject).GetComponent<Scrap>();
			scrap.value = value;
	}

	public int value;
	public bool beamed = false;
	
	Vector3 _rotate;

	// Use this for initialization
	void Start () {
		_rotate = new Vector3(0,0, Random.Range(-2f, 2f));
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(_rotate);
		transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * -0.2f);	
		if (Vector3.Distance(transform.position, Vector3.zero) > 20f || Gui.gameMode == 0) {
			Destroy(gameObject);
		}
	}
	
	public void Beam(Vector3 position, float speed) {
		beamed = true;
		transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);	
	}
}
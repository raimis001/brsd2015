using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public float speed;
	public float delay;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Gui.gameMode != 1) {
			Vector3 pos = transform.parent.position;
			pos.y = transform.position.y;
			transform.position = pos;
			return;
		}
		
		if (delay > 0) {
			delay -= Time.deltaTime;
			return;
		}
		
		transform.Translate(-speed * Time.deltaTime, 0, 0);
		if (transform.position.x < -15) {
			Vector3 pos = transform.parent.position;
			float camHalfHeight = Camera.main.orthographicSize;
			pos.y = Random.Range(-camHalfHeight, camHalfHeight);
			transform.position = pos;
		}
	}
}

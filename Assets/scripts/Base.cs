using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour {


	public GameObject basement;
	public GameObject roof;

	public Vector3 delta = new Vector3(0,0,0);
	
	public static Base instance;
	
	// Use this for initialization
	void Start () {
		instance = this;
			
		Begin();	
	}
	public void Begin() {
	
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		
		Vector3 topRightPosition = new Vector3(camHalfWidth, camHalfHeight / 2f, 0) + Camera.main.transform.position; 
		
		Bounds bounds = basement.GetComponent<SpriteRenderer>().bounds;
		Bounds bounds1 = roof.GetComponent<SpriteRenderer>().bounds;
		topRightPosition += new Vector3(bounds.size.x / 2f + bounds1.size.x / 2,-bounds.size.y / 2f, 0);
		
		topRightPosition.z = 0;
		topRightPosition.y = 0;
		transform.position = topRightPosition + delta;
		
		
		gameObject.SetActive(true);
		roof.SetActive(true);
		GetComponent<AudioSource>().Play();
		//Debug.Log("Playe");
	}
	
	public static void StartBase() {
		if (instance != null) instance.Begin();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Gui.gameMode == 1 && gameObject.activeSelf) {
			if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
			transform.Translate(-5f * Time.deltaTime, 0, 0);
			if (transform.position.x < -20) {
				gameObject.SetActive(false);
			}
			return;
		}
	
	
		if (Gui.gameMode != 2) return;

		transform.Translate(-5f * Time.deltaTime, 0, 0);
		if (transform.position.x < -3) {
			transform.position = new Vector3(-3f, 0, 0);
			roof.SetActive(false);
			//GetComponent<AudioSource>().Pause();
			Gui.gameMode = 0;
		}
	}
}

using UnityEngine;
using System.Collections;
//using UnityEditor;

public class Stars : MonoBehaviour {

	public Sprite[] starSprites;

	private Star[] stars;

	// Use this for initialization
	void Start () {
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		
		Vector3 topRightPosition = new Vector3(camHalfWidth, camHalfHeight / 2f, 0) + Camera.main.transform.position; 
		
		topRightPosition.x += 2f;
		topRightPosition.z = 0;
		topRightPosition.y = 0;
		transform.position = topRightPosition;
		
	
		stars = new Star[20];
		
		for (int i = 0; i < 20; i++) {
			GameObject star = new GameObject("star");
				Vector3 pos = transform.position;
				pos.y = Random.Range(-camHalfHeight,camHalfHeight);
				star.transform.position = pos;
				star.transform.parent = transform;
				
				SpriteRenderer sr = star.AddComponent<SpriteRenderer>();
					sr.sprite = starSprites[0];
				
				stars[i] = star.gameObject.AddComponent<Star>();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		
		Vector3 topRightPosition = new Vector3(camHalfWidth, camHalfHeight / 2f, 0) + Camera.main.transform.position; 
		
		topRightPosition.x += 2f;
		topRightPosition.z = 0;
		topRightPosition.y = 0;
		transform.position = topRightPosition;
		/*
		if (Gui.gameMode == 1) {
			for (int i = 0; i < 20; i++) {
				stars[i].gameObject.transform.Translate(-5f * Time.deltaTime, 0, 0);
				if (stars[i].gameObject.transform.position.x < -20f) {
					Vector3 pos = transform.position;
					pos.y = Random.Range(-camHalfHeight,camHalfHeight);
					stars[i].gameObject.transform.position = pos;
				}
			}
		}
		*/
	}
	
	
	
}

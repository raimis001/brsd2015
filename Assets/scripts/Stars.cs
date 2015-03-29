using UnityEngine;
using System.Collections;
//using UnityEditor;

public class Stars : MonoBehaviour {


	private Transform[] stars;

	// Use this for initialization
	void Start () {
		stars = new Transform[20];
	}
	
	// Update is called once per frame
	void Update () {
	
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		
		Vector3 topRightPosition = new Vector3(camHalfWidth, camHalfHeight / 2f, 0) + Camera.main.transform.position; 
		
		
		topRightPosition.z = 0;
		topRightPosition.y = 0;
		transform.position = topRightPosition;
		
	}
	
	
	
}

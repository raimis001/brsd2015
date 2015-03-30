using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

	public Sprite[] planets;

	float _scale = 4.2f;

	protected static Planet instance;
	// Use this for initialization
	void Start () {
		instance = this;
		//SetScale();
		//SetPosition(new Vector3(instance.transform.parent.position.x + 20f, instance.transform.parent.position.y, 0));
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		SetScale();	
		if (Gui.gameMode == 0) {
			SetPosition(Vector3.zero);
		} else {
			SetPosition(new Vector3(instance.transform.parent.position.x + Camera.main.transform.position.x, Camera.main.transform.position.y, 0));
		}
	}

	
	void SetScale() {
		float camHalfHeight = Camera.main.orthographicSize;
		transform.localScale = new Vector3(camHalfHeight / _scale, camHalfHeight / _scale, 1f);
	}			
	void SetPosition(Vector3 delta) {
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		
		Vector3 topRightPosition = new Vector3(camHalfWidth, camHalfHeight, 0) + Camera.main.transform.position + delta; 
		
		Bounds bounds = GetComponent<SpriteRenderer>().bounds;
		topRightPosition += new Vector3(-bounds.size.x / 2f,-bounds.size.y / 2f, 0) ;
		
		topRightPosition.z = 0;
		transform.position = topRightPosition;
	}
	
	public static void UpdateLevel() {	
		Debug.Log("Update planet level: " + ShipData.currentLevel.ToString());
		if (!instance) return;
		
		SpriteRenderer sr = instance.gameObject.GetComponent<SpriteRenderer>();
			sr.sprite = instance.planets[ShipData.currentLevel - 1];
		
		instance.SetScale();
		instance.SetPosition(new Vector3(instance.transform.parent.position.x + Camera.main.transform.position.x, Camera.main.transform.position.y, 0));
	}
}

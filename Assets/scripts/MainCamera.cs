using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class MainCamera : MonoBehaviour {

	public ParticleSystem stars;

	public static bool panMode = false;
	Vector3 _panPos;
	
	float _timeRock;
	float _timeRockMax = 1f;
	
	// Use this for initialization
	void Start () {
		_timeRock =_timeRockMax;
	}
	void LateUpdate () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			if (Camera.main.orthographicSize > 1) Camera.main.orthographicSize--;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			if (Camera.main.orthographicSize < 12) Camera.main.orthographicSize++;
		}
		
		
		
	}
	// Update is called once per frame
	void Update () {
		if (_timeRock > 0) {
			_timeRock -= Time.deltaTime;
			if (_timeRock <= 0) {
				//Rock.create();
				_timeRock =_timeRockMax;
			}
		}
	
		if (EventSystem.current.IsPointerOverGameObject()) return;
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButtonDown(0)) {
			_panPos = mousePos;
		}
		
		if (Input.GetMouseButton(0)) {
			Vector3 delta = _panPos - mousePos;
			
			if (Mathf.Abs(delta.x) > 0.1f || Mathf.Abs(delta.y) > 0.1f) {
				transform.Translate(delta);
				_panPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				panMode = true;
				return;
			}
			
		}
		
		if (Input.GetMouseButtonUp(0)) {
			if (panMode) {
				panMode = false;
				return;
			}
		}
		
		
		
	}
}

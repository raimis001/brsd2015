using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class MainCamera : MonoBehaviour {

	public ParticleSystem stars;

	public static bool panMode = false;
	Vector3 _panPos;
	
	// Use this for initialization
	void Start () {
		
	}
	void LateUpdate () {
		
		
	}
	// Update is called once per frame
	void Update () {
	
		if (Gui.gameMode == 2) {
			if (Camera.main.orthographicSize != 5f) {
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5f, 0.01f);
			}
			return;
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
		
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			if (Camera.main.orthographicSize > 3) Camera.main.orthographicSize -= 0.5f;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			if (Camera.main.orthographicSize < 10) Camera.main.orthographicSize += 0.5f;
		}
		
		
		
	}
}

using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	public GameObject intro1;
	public GameObject intro2;
	public GameObject intro3;
	public GameObject intro4;
	
	public GameObject button;
	

	float startTime = 0;
	int _currentIntro = -1;
	int currentIntro {
		get { return _currentIntro; }
		set { 
			_currentIntro = value;
			switch (_currentIntro) {
				case 0:
					startTime = 0f;
					button.SetActive(false);
					break;
				case 1:
					startTime = 3.1f;
					intro1.SetActive(true);
					break;
				case 2:
					startTime = 8.1f;
					intro2.SetActive(true);
					break;
				case 3:
					startTime = 13.1f;
					intro3.SetActive(true);
					break;
				case 4:
					startTime = 18.1f;
					intro4.SetActive(true);
					break;
				case 5:
					StartGame();
					break;
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_currentIntro < 0) {
			return;
		}
	
		if (Input.GetMouseButtonDown(0)) {
			currentIntro++;
			return;
		}
	
		startTime += Time.deltaTime;
		if (currentIntro == 0) {
			if (startTime > 1.5) {
				currentIntro = 1;
			}
		} else if (currentIntro == 1) {
			if (startTime > 8) {
				currentIntro = 2;
			}
		} else if (currentIntro == 2) {
			if (startTime > 13) {
				currentIntro = 3;
			}
		} else if (currentIntro == 3) {
			if (startTime > 18) {
				currentIntro = 4;
			}
		} else if (startTime > 25) {
			StartGame();
		}
		
	}
	
	public void StartGame() {
		Application.LoadLevel("mainScene");
	}
	
	public void InitIntro() {
		currentIntro = 0;
	}
}


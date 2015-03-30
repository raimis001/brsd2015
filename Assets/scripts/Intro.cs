using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	public GameObject intro1;
	public GameObject intro2;
	public GameObject intro3;
	

	float startTime = 0;
	int currentItro = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		startTime += Time.deltaTime;
		if (currentItro == 0) {
			if (startTime > 5) {
				intro1.SetActive(true);
				currentItro = 1;
			}
		} else if (currentItro == 1) {
			if (startTime > 10) {
				intro2.SetActive(true);
				currentItro = 2;
			}
		} else if (currentItro == 2) {
			if (startTime > 15) {
				intro3.SetActive(true);
				currentItro = 3;
			}
		} else if (currentItro == 3) {
			if (startTime > 20) {
				currentItro = 4;
				StartGame();
			}
		}
	}
	
	public void StartGame() {
		Application.LoadLevel("mainScene");
	}
}


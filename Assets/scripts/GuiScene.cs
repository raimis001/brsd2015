using UnityEngine;
using System.Collections;

public class GuiScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StartGame() {
		Application.LoadLevel("mainScene");
	}
	
	public void EndGame() {
		Application.Quit();
	}
	
	public void StartIntro() {
		Application.LoadLevel("introScene");
	}
}

using UnityEngine;
using System.Collections;
//using UnityEditor;

public class Stars : MonoBehaviour {


	float _height = 0;
	// Use this for initialization
	void Start () {
		/*
		SerializedObject so = new SerializedObject(GetComponent<ParticleSystem>());
		SerializedProperty it = so.GetIterator();
		while (it.Next(true))
			if (it.propertyPath.IndexOf("Shape") > -1)
			Debug.Log (it.propertyPath);
		*/
		setSize();
	}
	
	// Update is called once per frame
	void Update () {
		setSize();
	}
	
	void setSize() {
		Vector3 campo = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
		if (_height == campo.y) return;
		
		float worldScreenHeight = Camera.main.orthographicSize * 2f;
		float worldScreenWidth = worldScreenHeight / (float)Screen.height * (float)Screen.width;
		
		
		_height = worldScreenWidth;
		/*
		SerializedObject so = new SerializedObject(GetComponent<ParticleSystem>());
			so.FindProperty("ShapeModule.boxY").floatValue = worldScreenWidth;
			so.ApplyModifiedProperties();
		*/
		transform.position = new Vector3(campo.x, Camera.main.transform.position.y, 0);
		
	}
}

using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {

	private static float explodeDelay = 0f;

	public static void create(Vector3 position, bool delay) {
		
		GameObject obj = Instantiate(Resources.Load("Explode"), position, Quaternion.identity) as GameObject;
		
		if (delay) {
			Explode explode = obj.GetComponent<Explode>();
			explode.delay = explodeDelay;
		
			explodeDelay += 0.2f;
		}
		
	}


	public float delay = 0f;
	public bool delayed = false;
	
	Animator anim;
	SpriteRenderer render;
	// Use this for initialization
	void Start () {
		delayed = delay > 0f;
		anim = GetComponent<Animator>();
		render = GetComponent<SpriteRenderer>();
		if (delayed) {
			render.enabled = false;
			anim.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!delayed || anim.enabled) return;
		if (delay > 0f) {
			delay -= Time.deltaTime;
			if (delay <= 0f) {
				render.enabled = true;
				anim.enabled = true;
			}
		}
		
	}
	
	public void endAnimation(float theValue) {
		if (delayed) {
			explodeDelay -= 0.2f;
			if (explodeDelay < 0) explodeDelay = 0f;
		}
		Destroy(gameObject);
	}
		
}

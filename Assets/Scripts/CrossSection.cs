using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSection : MonoBehaviour {

public float start = 25;
public float width = 5;
public float time = 3f;
Camera camera;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
	{
//		camera.nearClipPlane = Mathf.Lerp(start, 0, (Time.time/time) % 1f);
//		camera.farClipPlane = Mathf.Lerp(start + width, width, (Time.time/time) % 1f);
		camera.farClipPlane = Mathf.Lerp(start, 0 + width, Mathf.PingPong(Time.time/time, 1f));
	}
}

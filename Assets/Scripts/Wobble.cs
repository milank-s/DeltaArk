using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour {

	public float offset;
	public float speed;
	public float size;
	public Vector3 axis = Vector3.one;
	float curSize;
	Vector3 origSize;
	// Use this for initialization
	void Start () {
		origSize = transform.localScale;
	}

	// Update is called once per frame
	void Update () {
		curSize = (Mathf.Sin(Time.time * speed + offset) * size);
		transform.localScale = new Vector3(origSize.x + (axis.x * curSize), origSize.y + (axis.y * curSize), (axis.z * curSize));

	}
}

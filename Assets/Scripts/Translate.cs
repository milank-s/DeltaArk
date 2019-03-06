using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {

	public Vector3 destination;
	private Vector3 pos;
	public float speed;
	// Use this for initialization
	void Start(){
		pos = transform.position;
	}
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, pos + destination) > 0.1f){
		transform.position = Vector3.MoveTowards(transform.position, pos + destination, speed * Time.deltaTime);
	}else{
		transform.position = pos;
	}
}
}

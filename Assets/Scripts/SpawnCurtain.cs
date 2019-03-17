using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCurtain : MonoBehaviour {

	public GameObject curtain;
	public int width;
	public int height;
	public float distance;

	// Use this for initialization
	void Start () {
		transform.position -= width/2 * Vector3.right * distance;
		transform.position += Vector3.up * height/2;
		for(int i = 0; i < 4; i++){
			GameObject newCurtain = (GameObject)Instantiate(curtain, transform.position, Quaternion.identity);
			newCurtain.transform.parent = transform;
			if(i != 0){
				switch(i){
					case 1:
					newCurtain.transform.position += transform.right * (width -1) * distance;
					newCurtain.transform.position += transform.forward * (width -1) * distance;
					newCurtain.transform.Rotate(0,90,0);
					break;

					case 2:
					newCurtain.transform.Rotate(0,180,0);
					newCurtain.transform.position += transform.forward * (width -1) * distance;
					newCurtain.transform.position += transform.right * (width -1) * distance;
					break;

					case 3:
					newCurtain.transform.Rotate(0,270,0);
					break;
				}
			}
			Curtain c = newCurtain.GetComponent<Curtain>();
			c.width = width;
			c.height = height;
			c.distance = distance;
			c.Initialize();
		}
	}

	// Update is called once per frame
	void Update () {

	}
}

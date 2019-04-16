using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainImages : MonoBehaviour {

	public Texture[] images;
	private Material mat;
	public bool switchImage;
	public float speed;
	float timer;
	int index;
	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().material;
		timer = speed;
	}

	// Update is called once per frame
	void Update () {
		if(timer < 0 && switchImage){
			index++;
			mat.mainTexture = images[index % images.Length];
			timer = speed;
		}
		timer -= Time.deltaTime;
		mat.SetTextureOffset("_MainTex", new Vector2(Time.time/3f, 0));
	}
}

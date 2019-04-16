using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BargeImages : MonoBehaviour {

	public Material mat;
	public Texture[] images;
	public Gradient colors;
	public bool switchImage;
	public float distortion;
	public float rotation;
	public float speed;
	Vector3 rVector;
	Vector3 fVector;
	Vector3 r2Vector;
	Vector3 f2Vector;
	float timer;
	bool stopAnim = false;
	int index;
	GameObject wordObject;
	private List<Transform> words;
	Vector3 originalPos;
	// Use this for initialization
	[ExecuteInEditMode]

	void Start () {
		words = new List<Transform>();
		foreach(Renderer r in GetComponentsInChildren<Renderer>()){
			GameObject newWord = (GameObject)Instantiate(Resources.Load<GameObject>("Text"), r.transform.position + Vector3.right/5f, r.transform.rotation);
			newWord.transform.forward = r.transform.right;
			newWord.GetComponent<TextMesh>().text = ".";
			words.Add(newWord.transform);
			r.material = mat;
			r2Vector =	-Vector3.right;
			f2Vector =  -Vector3.forward;
			rVector = r.transform.up;
			fVector = r.transform.forward;
			// r.transform.up +=  Vector3.right;
			r.transform.Rotate(0,0,Random.Range(-rotation, rotation));

			float t= Random.Range(0f, 1f);
			Color c = colors.Evaluate(t);
			timer = 1;
			// r.transform.localScale *= Random.Range(0.8f, 1.2f);
			// r.transform.position += r.transform.up * Random.Range(-0.02f, 0.02f);
			r.material.color = new Color(c.r,c.r, c.r, r.material.color.a);

			// r.material.SetColor("_TintColor", new Color(c.r,c.r, c.r, r.material.GetColor("_TintColor").a));
		}
	}

	// Update is called once per frame
	void Update () {
		// if(timer < 0 && switchImage){
		// 	index++;
		// 	mat.mainTexture = images[index % images.Length];
		// 	timer = speed;
		// }
		int t= 0;
		foreach(Renderer r in GetComponentsInChildren<Renderer>()){
			words[t].position = r.transform.position + -r.transform.right/5f + r.GetComponent<MeshFilter>().mesh.bounds.extents.y/2 * transform.up;
			t++;

			r.material.SetTextureOffset("_MainTex", new Vector2(-Time.time + Mathf.PerlinNoise(t/2f, 0) * Mathf.PerlinNoise(t/10f, 0) * 10,0));
			// -Time.time/4 + Mathf.PerlinNoise(t/10f, 0) * Mathf.PerlinNoise(t/10f, 0) * 2)
			float s = Mathf.Sin(Time.time + t);
			if(Mathf.Abs(s) < 0.05f && stopAnim == false){
				stopAnim = true;
			}

			// r.transform.up = Vector3.Lerp(rVector, r2Vector, Easing.SineEaseOut(Mathf.PingPong(Time.time/10, 1)));
			// r.transform.forward= Vector3.Lerp(fVector, f2Vector, Easing.SineEaseIn(Mathf.PingPong(Time.time/10, 1)));
			// r.transform.forward = new Vector3(Easing.SineEaseIn(Mathf.PingPong(Time.time/10, 1)), Easing.SineEaseIn(Mathf.PingPong(Time.time/1, 1)), Mathf.Sin(Time.time));
			// r.transform.up = new Vector3( Mathf.Sin(Time.time), Easing.SineEaseIn(Mathf.PingPong(Time.time/10, 1)), Easing.SineEaseIn(Mathf.PingPong(Time.time/1, 1)));

			if(stopAnim && timer > 0){
				timer -= Time.deltaTime;
				if(timer <= 0){
					stopAnim = false;
					timer = 1;
				}
			}

			if(!stopAnim){

				r.transform.position += Mathf.Pow(s, 3) * Time.deltaTime * distortion *  r.transform.up /10f  ;
				r.transform.position += Mathf.Pow(s, 3) * Time.deltaTime * distortion * r.transform.right/10f ;
			}

		}
	}
}

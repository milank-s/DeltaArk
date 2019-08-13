using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconImages : MonoBehaviour
{

	public int startIndex;
	public float brightness = 0.5f;
	private float timeOffset;
	private TextMesh text;
	public Texture[] images;
	private Material mat;
	public Color c;
	public bool switchImage;
	public float speed;
	public float alphaSpeed;
	public float scrollSpeed;
	private float timer;
	private float alpha;
	public float verticalScroll;
	int index;
	public bool noAlternate = false;

	public bool off;
	// Use this for initialization
	void Start ()
	{
		text = GetComponentInChildren<TextMesh>();
		mat = GetComponent<Renderer>().material;
		timeOffset = Random.Range(0, Mathf.PI * 2);
		
		
		if (off)
		{
			startIndex = 1;
			timer = speed*2 - 0.5f;
		}
		else
		{
			timer = speed;
		}
		
		index = startIndex;
		
		mat.mainTexture = images[startIndex];
	}

	// Update is called once per frame
	void Update () {
		if(timer < 0f && switchImage){
			index +=2 ;
			mat.mainTexture = images[index % images.Length];
			if (!noAlternate)
			{
				timer = speed * 2 - 1;
			}
			else
			{
				timer = speed;
			}
		}

		alpha = Mathf.Clamp01(timer / speed);

		if (!noAlternate)
		{
			mat.SetColor("_TintColor", new Color(c.r, c.g, c.b, Mathf.Sin(alpha * Mathf.PI) / 2));
		}
		else
		{
			mat.SetColor("_TintColor", new Color(c.r, c.g, c.b, alpha/2));
		}

		if (text != null)
		{
			text.color = new Color(1,1,1, 0);	
		}
		timer -= Time.deltaTime;
		mat.SetTextureOffset("_MainTex", new Vector2(Time.time * scrollSpeed, mat.GetTextureOffset("_MainTex").y + verticalScroll * Time.deltaTime));
		//(-alpha + 0.5f) * 0)
	}
}

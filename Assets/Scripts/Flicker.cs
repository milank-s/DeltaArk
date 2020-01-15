using System.Collections;
using Boo.Lang;
using UnityEngine;

public class Flicker : MonoBehaviour
{
	private List<Renderer> r;

	public bool randomize = false;
	public float speed = 3;
	public float alpha = 1;
	public bool gradient = true;

	private float offset;
	
	// Use this for initialization
	void Start ()
	{
		r = new List<Renderer>();
		foreach (Renderer rend in GetComponentsInChildren<Renderer>())
		{
			r.Add(rend);
		}

		offset = Random.Range(0, Mathf.PI);

	 Update();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float lerp = 0;
		
		if(gradient)
		{
			lerp = Mathf.Sin((Time.time + offset) / speed) + 1;
		}
		else
		{
			
			lerp = Mathf.Sin((Time.time + offset) / speed);
			if (lerp < 0)
			{
				lerp = 0;
			}
			else
			{
				lerp = alpha;
			}
		}

		
		foreach (Renderer rend in r)
		{
			rend.material.color = Color.Lerp(new Color(1, 1, 1, 0f), new Color(1, 1, 1, alpha),   lerp);
		}
	}
}

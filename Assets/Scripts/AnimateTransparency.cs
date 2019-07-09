using System.Collections;
using Boo.Lang;
using UnityEngine;

public class AnimateTransparency : MonoBehaviour
{
	private List<Renderer> r;

	public bool reverse;
	
	public float distance = 10;
	public float speed = 3;
	// Use this for initialization
	void Start ()
	{
		r = new List<Renderer>();
		foreach (Renderer rend in GetComponentsInChildren<Renderer>())
		{
			r.Add(rend);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		float lerp = 0;
		if (reverse)
		{
			 lerp = 1- (Mathf.Sin(((Time.time / speed) * Mathf.PI / 2) % Mathf.PI / 2));
		}
		else
		{
			lerp = (Mathf.Sin(((Time.time / speed) * Mathf.PI / 2) % Mathf.PI / 2));
		}

		transform.position = Vector3.forward * distance * (1- Mathf.Pow(lerp, 0.5f));
		foreach (Renderer rend in r)
		{
			rend.material.color = Color.Lerp(new Color(1, 1, 1, 0f), new Color(1, 1, 1, 0.25f),   lerp);
		}
	}
}

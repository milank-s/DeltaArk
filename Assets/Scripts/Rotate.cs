using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

	public bool local;
	public Vector3 rot;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (!local)
		{
			transform.Rotate(rot.x * Time.deltaTime, rot.y * Time.deltaTime, rot.z * Time.deltaTime, Space.World);
		}
		else
		{
			
			transform.Rotate(rot.x * Time.deltaTime, rot.y * Time.deltaTime, rot.z * Time.deltaTime);
		}
	}
}

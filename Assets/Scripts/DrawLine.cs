using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class DrawLine : MonoBehaviour
{

	public List<Transform> targets;

	private MeshFilter r;
	
	private int offset;
	private int index;

	public Mesh[] meshes;
	
	public float speed = 0.33f;
	private float timer;
	private LineRenderer l;

	// Use this for initialization
	void Start()
	{
		r = GetComponent<MeshFilter>();
		timer = Random.Range(0, speed);
		offset = Random.Range(0, targets.Count);
		l = GetComponent<LineRenderer>();
		l.positionCount = 2;
//		l.positionCount = targets.Count * 2;
//		for (int i = 0; i < targets.Count; i++)
//		{
//			l.SetPosition(i * 2, targets[i].position);
//			l.SetPosition(i * 2 + 1, transform.position);
//		}
	}

	void Update()
	{
		if (timer < 0)
		{
			if (index >= targets.Count)
			{
				index = 0;
			}
			
			for (int i = 0; i < index; i++)
			{
				l.SetPosition(0, targets[Random.Range(0, targets.Count)].position);
				l.SetPosition(1, transform.position);
			}

//			r.mesh = meshes[Random.Range(0, meshes.Count)];
			index = Random.Range(0, targets.Count);
			timer = Random.Range(0, speed);
			
			
			int rotation = Random.Range(0, 6);

			Vector3 newDir = Vector3.forward;
			switch (rotation)
			{
				case 0:
					newDir = Vector3.up;
					break;
			
				case 1:
					newDir = -Vector3.up;
					break;
			
				case 2:
					newDir = Vector3.right;
					break;
			
				case 3:
					newDir = -Vector3.right;
					break;
			
				case 4:
					newDir = Vector3.forward;
					break;
			
				case 5:
					newDir = -Vector3.forward;
					break;
			
			}
//			transform.forward = transform.parent.TransformDirection(newDir);
		}



		
		timer -= Time.deltaTime;
	}
}

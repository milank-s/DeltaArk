﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

// [ExecuteInEditMode]
public class LineRender3DObject : MonoBehaviour {

	bool fromFile;
	Mesh mesh;
	private MeshFilter r;
	public TextAsset shapeFile;
	List<Vector3> shape;
	VectorLine line;
	private float offset;
	public bool isStatic;

	public bool changeColor;
	public Color color;
	// Use this for initialization
	void Start ()
	{
		if(GetComponent<MeshFilter>() == null)
		{
			Destroy(this);
		}
		
		offset = Random.Range(0f, Mathf.PI);
			shape = new List<Vector3>();

			if(shapeFile == null){
				fromFile = false;
				r = GetComponent<MeshFilter>();
				mesh = r.mesh;
				GetComponent<MeshRenderer>().enabled = false;
				foreach(Vector3 v in mesh.vertices){
					shape.Add(v)	;
				}
			}else{
				fromFile = true;
			 	shape = VectorLine.BytesToVector3List (shapeFile.bytes);
			}

			line = new VectorLine (gameObject.name, shape, 0.5f , LineType.Continuous, Vectrosity.Joins.Weld);
			line.color = color;
			line.smoothWidth = true;
			line.smoothColor = true;
			
			for (int i = 0; i < shape.Count; i++)
			{
				if (!fromFile)
				{
					line.points3[i] = transform.TransformPoint(mesh.vertices[i]);

				}
				else
				{
					Vector3 newPoint = transform.TransformPoint(shape[i]);
					if (i % 2 == 0)
					{
						// newPoint = newPoint + Vector3.forward * Mathf.Sin(Time.time * 5f + (float)i/5f);
					}

					// newPoint = transform.TransformRotation(shape[i]);
					line.points3[i] = newPoint;
					if (i == 0)
					{
						// Debug.Log( transform.TransformPoint(shape[i]));
					}
				}

			}
			
			line.Draw3D();
	}

	// Update is called once per frame
	void Update () {
		if(fromFile){
			shape = VectorLine.BytesToVector3List (shapeFile.bytes);
		}
		
		if(!isStatic) {
		for (int i = 0; i < shape.Count; i++)
		{
			if (!fromFile)
			{
				line.points3[i] = transform.TransformPoint(mesh.vertices[i]);

			}
			else
			{
				Vector3 newPoint = transform.TransformPoint(shape[i]);
				if (i % 2 == 0)
				{
					// newPoint = newPoint + Vector3.forward * Mathf.Sin(Time.time * 5f + (float)i/5f);
				}

				// newPoint = transform.TransformRotation(shape[i]);
				line.points3[i] = newPoint;
				if (i == 0)
				{
					// Debug.Log( transform.TransformPoint(shape[i]));
				}
			}

			if (changeColor)
			{
				line.SetColor(Color.Lerp(color, new Color(0,0,0, 0) + Color.white * 0.5f, Mathf.PingPong(Time.time + offset, 1)), i);
				
			}
		}

		line.Draw3D();
		}
}
}

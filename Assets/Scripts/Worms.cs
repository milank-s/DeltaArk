using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using Vectrosity;

public class Worms : MonoBehaviour
{
	public float increment = 1;
	public float speed = 0.5f;
	public float distance = 1f;
	public int height = 5;
	public int width = 10;
	public GameObject child;
	public float offset;
	public Material lineMaterial;
	public Texture texture;
	public bool reverseY = false;
	public Color c = Color.blue;
	public Transform word;
	List<Transform> children;
	List<Vector3> pos;

	private float time;
	VectorLine line;
	// Use this for initialization

	void Start()
	{
		Initialize();
	}
	
	public void Initialize()
	{
		offset = Random.Range(-10000f, 10000f);

		pos = new List<Vector3>();
		children = new List<Transform>();

		line = new VectorLine(gameObject.name, new List<Vector3>(), 0.5f, LineType.Continuous,
			Vectrosity.Joins.Weld);
		line.color = c;
		line.smoothWidth = true;
		line.smoothColor = true;
					line.material = lineMaterial;
					line.texture = texture;

		for (int i = 0; i < width; i++)
		{

				GameObject newChild = new GameObject();
				newChild.transform.position = transform.position + Random.insideUnitSphere;
				newChild.transform.parent = transform;
				

				pos.Add(newChild.transform.position);
				children.Add(newChild.transform);
			}
	}

	// Update is called once per frame
	void Update () {
		
		for(int i = 0; i < children.Count; i++)
		{
			if (i == 0)
			{
				word.transform.position = children[i].position;
			}
			time = Time.time * speed + (Mathf.PerlinNoise( i / 10f - Time.time + offset, i / 10f + Time.time + offset) * Time.deltaTime);
			
			
			float x = Mathf.PerlinNoise((float) i / increment - offset - time, (float) i / increment  + offset - time) * 2f;
			float y = 0;
			if (reverseY)
			{
				y = Mathf.PerlinNoise((float) i / increment + offset + time,
					          (float) i / increment - offset + time) * 2f;
				
			}
			else
			{
				y = Mathf.PerlinNoise((float) i / increment + offset - time,
					          (float) i / increment - offset - time) * 2f;
			}
			float z = Mathf.PerlinNoise((float) i / increment + offset - time,
				          (float) i / increment - offset - time) * 2f;
			children[i].transform.position = transform.position + new Vector3(x - 0.75f, y- 0.75f, 0 - 0.75f) * distance;
			pos[i] = children[i].transform.position;
			for(int j = 0; j < children.Count; j++){
				
				int k = (i * height) + j;
			//move points along z axis
//			children[k].position = pos[k] + (Vector3.forward * 2 * Mathf.Sin(Time.time + (float)(j)/2f + i/2) * (float)j * 0.5f/(float)height);
			//move points along x axis
//			children[k].position = pos[k] + (Vector3.right * 2 * Mathf.Sin(Time.time + i/2f) * (float)j * 0.5f/(float)height);
			
		}
	}

		line.points3 = pos;
		line.Draw3D();
	}
}

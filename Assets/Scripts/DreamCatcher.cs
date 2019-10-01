using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class DreamCatcher : MonoBehaviour
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
	public RectTransform[] words;
	public Transform word;
	public UnityEngine.UI.Image[] image;
	List<Transform> children;
	List<Vector3> pos;

	private float time;
	VectorLine line;

	private List<VectorLine> lines;
	// Use this for initialization

	void Start()
	{
		Initialize();
	}
	
	public void Initialize()
	{
		lines = new List<VectorLine>();
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
			lines.Add(new VectorLine(gameObject.name, new List<Vector3>(), 0.5f, LineType.Continuous,
				Vectrosity.Joins.Weld));
			lines[i].color = c;
			lines[i].smoothWidth = true;
			lines[i].smoothColor = true;
			lines[i].material = lineMaterial;
			lines[i].texture = texture;

				GameObject newChild = new GameObject();
				newChild.transform.position = transform.position + Random.insideUnitSphere;
				newChild.transform.parent = transform;
				

				pos.Add(newChild.transform.position);
				children.Add(newChild.transform);
			}
	}

	// Update is called once per frame
	void Update ()
	{
		bool yes = false;
		//distance = Mathf.Sin(Time.time) + 2f;
		for (int i = 0; i < image.Length; i++)
		{
			image[i].fillAmount = Mathf.PingPong(Time.time / 10f, 1);
			yes = !yes;
			if (yes)
			{
				words[i].Rotate(0, 0, 10 * Time.deltaTime);
			}
			else
			{
				words[i].Rotate(0, 0, 10 * -Time.deltaTime);
			}
		}

	
		for(int i = 0; i < children.Count; i++)
		{
			List<Vector3> points = new List<Vector3>();
			
			time = Time.time * speed + (Mathf.PerlinNoise( i / 10f - Time.time + offset, i / 10f + Time.time + offset) * Time.deltaTime);
			
			
			float x = Mathf.PerlinNoise((float) i / increment - offset + time, (float) i / increment  + offset + time) * 2f;
			float z = Mathf.PerlinNoise((float) i / increment + offset + time, (float) i / increment  - offset + time) * 2f;
			
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
			
			Vector3 newPos = new Vector3(x - 0.9f, y- 0.9f, 0) * distance;
			if (newPos.magnitude > 1)
			{
				newPos = newPos.normalized;
			}
			children[i].localPosition = newPos;
			pos[i] = children[i].transform.position;
//			increment += Mathf.Sin(Time.time) * Time.deltaTime;
			points.Add(children[i].position);
			//move points along z axis
//			children[k].position = pos[k] + (Vector3.forward * 2 * Mathf.Sin(Time.time + (float)(j)/2f + i/2) * (float)j * 0.5f/(float)height);
			//move points along x axis
//			children[k].position = pos[k] + (Vector3.right * 2 * Mathf.Sin(Time.time + i/2f) * (float)j * 0.5f/(float)height);

				for (int j = children.Count-1; j > 0 ; j--)
				{
					if(Vector3.Distance(children[j].transform.position, children[i].transform.position) < 0.55f)
					{
						points.Add(children[j].position);
						points.Add(children[i].position);
					}
					
				}

				lines[i].points3 = points;
				lines[i].Draw3D();
		}
		
		//transform.Rotate(0,10f * Time.deltaTime,10f * Time.deltaTime);

//		line.Draw3D();
	}
}

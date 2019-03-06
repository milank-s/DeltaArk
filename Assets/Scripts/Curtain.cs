using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Curtain : MonoBehaviour {

	public float childDistance = 1f;
	public int height = 5;
	public int width = 10;
	public GameObject child;

	List<Transform> children;
	List<Vector3> pos;
	List<Transform> edge;
	VectorLine line;
	List<VectorLine> creases;
	// Use this for initialization

	public void Initialize(){
		children = new List<Transform>();
		edge = new List<Transform>();
		creases = new List<VectorLine>();
		pos = new List<Vector3>();
		List<Vector3> positions = new List<Vector3>();
		// positions.Add(transform.position);
		for(int i = 0; i < width; i++){

			List<Vector3> creasePositions = new List<Vector3>();

			for(int j = 0; j < height; j++){
			GameObject newChild = (GameObject)Instantiate(child, transform.position - transform.up * (j) + transform.right * (i), Quaternion.identity);
			if(j != 0){
				newChild.transform.parent = children[i * height];
			}else{
				newChild.transform.parent = transform;
			}
			pos.Add(newChild.transform.position);
			children.Add(newChild.transform);

			if(i == 0 || j == height-1 || i == width -1){
				positions.Add(newChild.transform.position);
				edge.Add(newChild.transform);
			}
			creasePositions.Add(newChild.transform.position);
		}

		if(i > 0 && i < width -1){
		 VectorLine v = new VectorLine (gameObject.name, creasePositions, 1 , LineType.Continuous, Vectrosity.Joins.Weld);
			v.color = Color.white;
			v.smoothWidth = true;
			v.smoothColor = true;
			creases.Add(v);
		}
	}
		int index = width + height - 2;
		edge.Reverse(index, height);
		line = new VectorLine (gameObject.name, positions, 1 , LineType.Continuous, Vectrosity.Joins.Weld);
		line.color = Color.white;
		line.smoothWidth = true;
		line.smoothColor = true;
	}

	// Update is called once per frame
	void Update () {
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				int k = (i * height) + j;
			//move points along z axis
			children[k].position = pos[k] + (Vector3.forward * Mathf.Sin(Time.time + (float)(j)/2f + i) * (float)j * 0.5f/(float)height);
			//move points along x axis
			children[k].position = pos[k] + (Vector3.right * Mathf.Sin(Time.time + i/2f) * (float)j * 0.5f/(float)height);
			if(i > 0 && i < width -1){

				if(j >= creases[i-1].points3.Count){
					creases[i-1].points3.Add(children[k].position);
				}else{
					creases[i-1].points3[j] = children[k].position;
				}

				if(j < height - 1){
					float c = 1-  ((float)j/(float)height);
					creases[i-1].SetColor(new Color(c,c,c),j);
				}
			}
		}
	}

		for(int j = 0; j < edge.Count; j++){
				line.points3[j] = edge[j].position;
		}
		foreach(VectorLine v in creases){
			v.Draw3D();
		}
		line.Draw3D();
	}
}

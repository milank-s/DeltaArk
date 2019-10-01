using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;
public class LightAtlas : MonoBehaviour
{

	public GameObject cityPrefab;
	private ParticleSystem p;
	public ParticleSystem cp;

	public Transform circle;
	private ParticleSystem.ShapeModule s;
	public float interval = 0.1f;
	private float time;
	private bool spawn;
	private ParticleSystemRenderer r;
	ParticleSystem.Particle[] m_Particles;
	private ParticleSystem.Particle[] circles;
	List<Vector3> pos;
	VectorLine line;
	// Use this for initialization
	void Start (){
		r = GetComponent<ParticleSystemRenderer>();
		p = GetComponent<ParticleSystem>();
		
		s = p.shape;
		time = 0;
		p.Stop();
		//s.arcSpread = (Mathf.Sin(Time.time) + 1f) / 2f;
		//s.radiusSpread = (Mathf.Sin(Time.time) + 1f) / 2f;
		p.Play();
		//r.material.SetColor("_TintColor",Color.black);
		m_Particles = new ParticleSystem.Particle[p.main.maxParticles];
		circles = new ParticleSystem.Particle[cp.main.maxParticles];
		pos = new List<Vector3>();
		for (int i = 0; i < m_Particles.Length; i++)
		{
			pos.Add(m_Particles[i].position);	
		}
		
		line = new VectorLine (gameObject.name, pos, 1, LineType.Continuous, Vectrosity.Joins.Weld);
		line.color = Color.white;
		line.smoothWidth = true;
		line.smoothColor = true;
		// f.texture = texture;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (time > interval/2)
		{
			if (!spawn)
			{
				
				//Instantiate(cityPrefab, transform.position, Quaternion.identity);
				spawn = true;
			}
		
			
			
//			time = interval;
//			s.radiusSpread = (Mathf.Sin(Time.time) + 1f) / 2f;
//			s.radiusSpread = Mathf.PerlinNoise(Time.time, -Time.time);
//			s.radiusSpread = Random.Range(0, 0.99f);
		
//			s.radius = (Mathf.Sin(Time.time) + 1f) / 2f;
//			p.Stop();
//			enabled = false;
			if (time >= interval)
			{
				//Destroy(gameObject);
			}
		}


		Vector3 closestParticle = new Vector3(0, 0, float.PositiveInfinity);
		//m_Particles = new ParticleSystem.Particle[p.GetParticles(m_Particles)];
		p.GetParticles(m_Particles);    
		cp.GetParticles(circles);    
		pos.Clear();

		for (int i = m_Particles.Length - 1; i >= 0; i--)
		{
			float z = m_Particles[i].position.z;
			if (m_Particles[i].position.x > (-Mathf.Sin(Time.time+ i/100) * z/5) - 1f && m_Particles[i].position.x < (-Mathf.Sin(Time.time + i/100) * z/5) + 1f && m_Particles[i].position.z > 1)
			{
				if (m_Particles[i].position.z < closestParticle.z)
				{
					closestParticle = m_Particles[i].position;
				}

				m_Particles[i].startColor = Color.white/10;
				pos.Add(m_Particles[i].position);
				circles[i].position = pos[pos.Count -1];
				circles[i].startColor = Color.white;
				//line.SetColor(Color.black, i);
			}
			
			
			
		}

		for (int i = 0; i < pos.Count; i++)
		{

			Vector3 val = Vector3.one * -1000;
			for (int j = 0; j < pos.Count-1; j++)
			{
				if (pos[j].z > pos[j + 1].z)
				{
					val = pos[j + 1];
					pos[j+1] = pos[j];
					pos[j] = val;
				}
			}
		}
		
		cp.SetParticles(circles, circles.Length);
		p.SetParticles(m_Particles,m_Particles.Length);
		//circle.position = closestParticle- Vector3.up * 2;

		line.points3 = pos;
		
		float c = Mathf.Sin(Mathf.Clamp(time * Mathf.PI, 0, Mathf.PI));
//		s.radiusSpread = (Mathf.Sin(Time.time/3f) + 1f) / 2f;
//		p.Stop();
//		p.Play();
//		s.scale = Vector3.right * 8f * (Mathf.Sin((Time.time - Mathf.PI)/2f) + 1f);
		
		//r.material.SetColor("_TintColor",new Color(c,c,c, 0.10f));
		line.Draw3D();
		time += Time.deltaTime;
	}
}

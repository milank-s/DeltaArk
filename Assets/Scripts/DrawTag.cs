using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTag : MonoBehaviour
{
    private Agent agent;
    private TextMesh t;
    private LineRenderer l;
    void Start()
    {
        agent = GetComponentInParent<Agent>();
        transform.position = agent.transform.position + Vector3.up;
        t = GetComponent<TextMesh>();
        l = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        t.text = agent.creatureType.ToString();
        t.text += "\n";
        t.text += "state = " + agent.state;
        t.text += "\n" + "\n";
        t.text += "h: " + agent.hunger.ToString("F1");
        t.text += "\n";
        t.text += "e: " + agent.energy.ToString("F1");
        
        l.SetPosition(0, transform.position);
        l.SetPosition(1, agent.transform.position);
    }
}

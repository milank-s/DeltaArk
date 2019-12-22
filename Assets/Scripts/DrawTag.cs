using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTag : MonoBehaviour
{
    private Agent agent;
    private TextMesh t;
    private LineRenderer l;

    private bool left, right, up, down;

    private  int agentIndex;
    private int creatureType;
    void Start()
    {
        agent = GetComponentInParent<Agent>();
        t = GetComponent<TextMesh>();
        l = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            creatureType--;
            if (creatureType < 0)
            {
                creatureType = AgentManager.creatureAmount-1;
            }

            agentIndex = 0;

        }else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            creatureType++;
            creatureType %= AgentManager.creatureAmount;
            agentIndex = 0;

        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            agentIndex--;
            if (agentIndex < 0)
            {
                agentIndex = AgentManager.entities[creatureType].Count - 1;
                
            }
            
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            agentIndex++;
            agentIndex %= AgentManager.entities[creatureType].Count;
        }

        if (AgentManager.entities[creatureType].Count > 0)
        {
            t.gameObject.SetActive(true);
            l.enabled = true;
            agent = AgentManager.entities[creatureType][agentIndex];
        }
        else
        {
            l.enabled = false;
            t.gameObject.SetActive(false);
            return;
        }

        t.text = agent.creatureType.ToString();
        t.text += "\n";
        t.text += "state = " + agent.state;
        t.text += "\n" + "\n";
        t.text += "h: " + agent.hunger.ToString("F1");
        t.text += "\n";
        t.text += "e: " + agent.energy.ToString("F1");


        l.SetPosition(0, l.transform.position);
        l.SetPosition(1, agent.transform.position);
    }
}

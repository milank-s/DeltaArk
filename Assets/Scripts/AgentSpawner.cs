using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{

    public List<GameObject> agentsToSpawn;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject g in agentsToSpawn)
        {
            GameObject newAgent = Instantiate(g, transform.position, Quaternion.identity);
            newAgent.GetComponent<Agent>().homeParent = transform;
        }
    }

}

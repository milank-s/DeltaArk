using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{

    public int numToSpawn = 1;
    public List<GameObject> agentsToSpawn;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject g in agentsToSpawn)
        {
            for (int i = 0; i < numToSpawn; i++) {
                GameObject newAgent = Instantiate(g, transform.position, Quaternion.identity);
                newAgent.GetComponent<Agent>().homeParent = transform;
            }
        }
    }

}

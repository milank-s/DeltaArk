using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{

    public GameObject tag;
    [SerializeField]
    public Agent[] AgentClasses;

    public static List<Agent>[] entities;
    
    public static List<Agent> GetCreatures(Agent.Creature c){
        return entities[(int) c];
    }

    void Awake()
    {
        entities = new List<Agent>[Enum.GetNames(typeof(Agent.Creature)).Length];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = new List<Agent>();
        }
        
    }
    
    
   
}

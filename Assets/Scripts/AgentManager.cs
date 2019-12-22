using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{

    public GameObject tag;
    [SerializeField]
    public Agent[] AgentClasses;

    public static int creatureAmount;
    public static List<Agent>[] entities;
    
    public static List<Agent> GetCreatures(Agent.Creature c){
        return entities[(int) c];
    }

    public static List<Agent.Creature>[] predatorLookup;
    

    void Awake()
    {
       creatureAmount = Enum.GetNames(typeof(Agent.Creature)).Length;
        entities = new List<Agent>[creatureAmount];
        predatorLookup = new List<Agent.Creature>[creatureAmount];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = new List<Agent>();
            predatorLookup[i] = new List<Agent.Creature>();
        }
        
    }


    void Start()
    {
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].Count > 0)

            {
                Agent.Creature me = (Agent.Creature) i;
                
                for (int j = 0; j < entities.Length; j++)
                {
                    if (i != j && entities[j].Count > 0)

                    {
                        
                        Agent.Creature you = (Agent.Creature) j;
                        foreach (Agent.Creature c in entities[j][0].prey)
                        {
                            if (c == me && !predatorLookup[i].Contains(c))
                            {
                                predatorLookup[i].Add(you);
                            }

                        }

                        foreach (Agent.Creature c in entities[i][0].prey)
                        {
                            if (entities[j][0].prey.Contains(c) && !predatorLookup[i].Contains(you))
                            {
                                predatorLookup[i].Add(you);
                            }

                        }
                    }
                }

            }
        }
    }
    
    
   
}

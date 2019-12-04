using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Agent : MonoBehaviour
{

    public List<Agent> targets
    {
        get
        {
            if (prey.Count > 0)
            {
                return AgentManager.entities[(int) prey[0]];
            }

            return null;
        }
    }
    public Agent curTarget;

    [SerializeField]
    public List<Creature> predators;
    [SerializeField]
    public List<Creature> prey;
    [SerializeField]
    public Creature creatureType;
    
    public bool isAlive;
    
    public enum State{normal, hungry, scared, tired, sleep, dead}
    public enum Creature{lichen, drone, lowlife}

   
    [SerializeField]
    public State state;
    public float health = 1;
    public float hunger = 1;
    public float energy = 10;
    public float speed = 1;
    public float nutrition = 1;
    public float xOffset;
    public float yOffset;
    
    public Vector3 home;

    public Vector3 distanceToTarget
    {
        get { return curTarget.transform.position - transform.position; }
    }

    public void Start()
    {
        Init();
    }
    protected void Init()
    {
        home = transform.position;
        xOffset = Random.Range(-9999f, 9999f);
        yOffset = Random.Range(-9999f, 9999f);
        AgentManager.entities[(int)creatureType].Add(this);
        isAlive = true;

    }
    protected bool FindClosestTarget()
    {
        float distance = Mathf.Infinity;
        int index = 0;
        int i = 0;
        bool foundTarget = false;
        if (prey.Count <= 0) return false;
        
        foreach (Agent a in targets)
        {
            float curDistance = Vector3.Distance(transform.position, a.transform.position);
            if (curDistance < distance && a.isAlive)
            {
                distance = curDistance;
                index = i;
                foundTarget = true;
            }

            i++;
        }

        if (foundTarget)
        {
            curTarget = targets[index];
            return true;
        }
        
        

        return false;

    }

    protected void CatchTarget()
    {
        hunger = curTarget.nutrition;
        curTarget.Caught();
        curTarget.transform.parent = transform;
        ChangeState(State.normal);
        curTarget = null;
    }

    public void Caught()
    {
        
        ChangeState(State.dead);
    }
    protected void Pursue()
    {
        if (curTarget != null)
        {
            transform.position += (curTarget.transform.position - transform.position).normalized * Time.deltaTime * speed;
            if (!curTarget.isAlive)
            {
                if (!FindClosestTarget())
                {
                    ChangeState(State.tired);
                }
            }
        }

        if (distanceToTarget.magnitude < 0.01f)
        {
            CatchTarget();
        }
    }

    protected void Sleep()
    {
        energy += Time.deltaTime;
        if (energy > 1)
        {
            ChangeState(State.normal);
        }
    }
    
    protected void Flee()
    {
        transform.position += (home - transform.position).normalized * speed * Time.deltaTime;   
    }

    protected void Move()
    {
        float xDir = Mathf.PerlinNoise(Time.time * -xOffset, Time.time + -yOffset) - 0.5f;
        float yDir = Mathf.PerlinNoise(Time.time * xOffset, Time.time + yOffset) - 0.5f;
        Vector3 moveDir = new Vector3(xDir, yDir, 0);
        transform.position += moveDir * Time.deltaTime * speed;
    }

    protected void ChangeState(State s)
    {
        switch (s)
        {
            case State.dead:

                isAlive = false;
                break;
            
            case State.scared :
                
                
                break;
            
            case State.tired :
            
                break;
            
            case State.hungry:

                if (!FindClosestTarget())
                {
                    return;
                }
            
                break;
            
        }

        state = s;
    }
   
    void Update()
    {
        hunger -= Time.deltaTime;
        
        if (state != State.sleep)
        {
            energy -= Time.deltaTime;
        }
        

        switch (state)
        {
            case State.normal:
            
                Move();
                if (energy < 0)
                {
                    ChangeState(State.tired);
                }else if (hunger < 0)
                {
                    ChangeState(State.hungry);
                }
                
                break;
            
            case State.hungry :

                Pursue();
                
                break;
            
            case State.scared :
                
                Flee();
                break;
            
            case State.tired :
                Flee();
                break;
            
            case State.sleep:
                Sleep();
                break;
        }
    }
}

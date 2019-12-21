using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{

  
    public Agent curTarget;

    [SerializeField]
    public List<Creature> predators;
    [SerializeField]
    public List<Creature> prey;
    [SerializeField]
    public Creature creatureType;

    public Agent captor;
    public List<Agent> heldPrey;
    public List<Agent> capturedPrey;
    
    public bool isTargeted;
    public bool isAlive;
    public bool hasEaten;
    
    public enum State{normal, hungry, scared, tired, sleep, dead}
    public enum Creature{lichen, drone, lowlife, crawler, hunter}

   
    [SerializeField]
    public State state;
    public float health = 1;
    public float hunger = 1;
    public float energy = 10;
    public float speed = 1;
    public float nutrition = 1;
    public float xOffset;
    public float yOffset;
    public float capacity = 1;
    public float weight = 1;
    public float carriedWeight;
    public float turnRadius = 2;
    public bool scavenger;
    
    private Vector3 velocity;

    public Transform homeParent;

    private Vector3 homePos;
    public Vector3 home
    {
        get
        {
            if (homeParent != null)
            {
                return homeParent.position;
            }
            else
            {
                return homePos;
            }
        }
    }

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
        homePos = transform.position;
        xOffset = Random.Range(-9999f, 9999f);
        yOffset = Random.Range(-9999f, 9999f);
        AgentManager.entities[(int)creatureType].Add(this);
        isAlive = true;
        hunger = Random.Range(5f, 10f);
        energy = Random.Range(-1f, 1f);

    }
    protected bool FindClosestTarget()
    {
        float distance = Mathf.Infinity;
        int index = 0;
        int i = 0;
        bool foundTarget = false;
        if (prey.Count <= 0) return false;

        Agent newTarget = null;
        
        foreach (Creature p in prey)
        {
            foreach (Agent a in AgentManager.entities[(int) p])
            {
                float curDistance = Vector3.Distance(transform.position, a.transform.position);
                if (a != this && curDistance < distance && ((a.isAlive && !scavenger) || (!a.isAlive && scavenger)) &&
                    !a.isTargeted)
                {
                    if (a.captor != null && (a.captor == this || a.captor.creatureType == creatureType))
                    {

                    }
                    else
                    {
                        distance = curDistance;
                        newTarget = a;
                        foundTarget = true;
                    }
                }

                i++;
            }
        }

        if (foundTarget)
        {
            if (curTarget != null)
            {
                StopChasing();
            }

            curTarget = newTarget;
            curTarget.isTargeted = true;
            return true;
        }
        
        return false;

    }

    void StopChasing()    
    {
        curTarget.isTargeted = false;
        
        if (curTarget.state == State.scared)
        {
            curTarget.ChangeState(State.normal);
        }
        
        curTarget = null;
    }

    protected void CatchTarget()
    {
        hunger = curTarget.nutrition;
        energy = curTarget.nutrition;
        curTarget.Caught();
        curTarget.transform.parent = transform;
        carriedWeight += curTarget.weight;
        heldPrey.Add(curTarget);
        if (curTarget.captor != null)
        {
            curTarget.captor.capturedPrey.Remove(curTarget);
        }
        curTarget.captor = this;
        
        if (carriedWeight >= capacity)
        {

            ChangeState(State.tired);
        }
        else
        {
            ChangeState(State.normal);
        }

        hasEaten = true;
        
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
            Vector3 targetDir = (curTarget.transform.position - transform.position);
            if (targetDir.magnitude > 1)
            {
                targetDir.Normalize();
                if (curTarget.state != State.scared)
                {
                    curTarget.ChangeState(State.scared);
                }

                FindClosestTarget();
            }else if (targetDir.magnitude < 0.2f)
            {
                
                targetDir.Normalize();
            }
            
            velocity = Vector3.Lerp(velocity, targetDir * speed, Time.deltaTime * turnRadius);
            
//            if (!curTarget.isAlive)
//            {
//                if (!FindClosestTarget())
//                {
//                    ChangeState(State.tired);
//                }
//            }
        }

        if (distanceToTarget.magnitude < 0.1f)
        {
            
            CatchTarget();
        }
        
        if (energy < 0 && !hasEaten)
        {
            ChangeState(State.tired);
        }

        
    }

    protected void Sleep()
    {
        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 5);
        energy += Time.deltaTime * 10;
        if (energy > 10)
        {
            ChangeState(State.normal);
        }

        hasEaten = false;
        
    }

    protected void ReturnHome()
    {
        
        Vector3 targetDir = (home - transform.position);
        if (targetDir.magnitude > 1)
        {
            targetDir.Normalize();
        }
        
        velocity = Vector3.Lerp(velocity, targetDir * speed, Time.deltaTime * turnRadius);

        if (Vector3.Distance(home, transform.position) < 0.1f)
        {
            if (heldPrey.Count > 0)
            {
                DropTarget();
            }

            if (energy > 0)
            {
                ChangeState(State.normal);
            }
            else
            {
                ChangeState(State.sleep);
            }
        }
    }
    
    protected void Flee()
    {
        velocity = Vector3.Lerp(velocity, (home - transform.position).normalized * speed, Time.deltaTime * turnRadius);
        if (Vector3.Distance(home, transform.position) < 0.1f)
        {
            ChangeState(State.sleep);
        }
    }

    protected void Move()
    {
        float xDir = Mathf.PerlinNoise(Time.time * -xOffset, Time.time + -yOffset) - 0.5f;
        float yDir = Mathf.PerlinNoise(Time.time * xOffset, Time.time + yOffset) - 0.5f;
        Vector3 moveDir = new Vector3(xDir, yDir, 0) * 2;
        velocity = Vector3.Lerp(velocity, moveDir * speed, Time.deltaTime);
    }

    protected void DropTarget()
    {
        foreach (Agent a in heldPrey)
        {
            a.transform.parent = null;
            a.isTargeted = false;
            a.DropTarget();
            capturedPrey.Add(a);
            a.transform.position = home + Vector3.up * 0.1f * capturedPrey.Count;
        }

        
        heldPrey.Clear();
        carriedWeight = 0;
    }

    protected void DropPrey()
    {
//        ???
    }

    protected void LeaveState(State s)
    {
        switch (s)
        {

            case State.scared :
                
                
                break;
            
            case State.tired :
            
                break;
            
            case State.hungry:

                StopChasing();
            
                break;
            
        }
    }
    
    protected void ChangeState(State s)
    {
        LeaveState(state);
        
        switch (s)
        {
            case State.dead:
                velocity = Vector3.zero;
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

        if (hunger < -10)
        {
//            ChangeState(State.dead);
        }
        
        if (state != State.sleep && state != State.normal)
        {
            energy -= Time.deltaTime;
        }

        if (isAlive)
        {
            transform.position += velocity * Time.deltaTime;
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
                ReturnHome();
                break;
            
            case State.sleep:
                Sleep();
                break;
        }
    }
}

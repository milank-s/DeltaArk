using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Experimental.UIElements;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{

  
    public Agent curTarget;

    [SerializeField]
    public List<Creature> predators {

        get { return AgentManager.predatorLookup[(int) creatureType]; }
       
    }

    [SerializeField]
    public List<Creature> prey;
    [SerializeField]
    public Creature creatureType;

    public Agent captor;
    public Agent pursuer;
    public List<Agent> heldPrey;
    public List<Agent> capturedPrey;
    
    public bool isTargeted;
    public bool isAlive;
    public bool hasEaten;
    
    public enum State{normal, pursue, scared, tired, sleep, dead}
    public enum Creature{lichen, drone, lowlife, crawler, hunter}

    public enum Job {gather, defend}

    public Job job = Job.gather;
    
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

    public Vector3 distanceToHome
    {
        get { return home - transform.position; }
    }
    
    public Vector3 distanceToPursuer
    {
        get { return pursuer.transform.position - transform.position; }
    }
    
    public void Awake()
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
                if (a != this && a.job != Job.defend && curDistance < distance && ((a.isAlive && !scavenger) || (!a.isAlive && scavenger)) &&
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
            curTarget.pursuer = this;
            curTarget.isTargeted = true;
            return true;
        }
        
        return false;

    }
    
    protected bool FindClosestPredator()
    {
        float distance = Mathf.Infinity;
        int index = 0;
        int i = 0;
        bool foundTarget = false;
        if (predators.Count <= 0) return false;
    
        Agent newTarget = null;
        
        foreach (Creature p in predators)
        {
            foreach (Agent a in AgentManager.entities[(int) p])
            {
                float curDistance = Vector3.Distance(transform.position, a.transform.position);
                if (a != this && curDistance < distance && a.isAlive && !a.isTargeted)
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

        if (foundTarget && distance < 1)
        {
            if (curTarget != null)
            {
                StopChasing();
            }

            curTarget = newTarget;
            curTarget.pursuer = this;
            curTarget.isTargeted = true;
            return true;
        }
        
        return false;

    }
    

    void StopChasing()    
    {
        curTarget.isTargeted = false;
        curTarget.pursuer = null;
        
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
            if (curTarget.captor.heldPrey.Contains(curTarget))
            {
                
                curTarget.captor.heldPrey.Remove(curTarget);
            }else if (curTarget.captor.capturedPrey.Contains(curTarget))
            {
                curTarget.captor.capturedPrey.Remove(curTarget);
            }
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
                
            }else if (targetDir.magnitude > 1)
            {
                if (curTarget.state != State.scared)
                {
                    curTarget.ChangeState(State.scared);
                }
            }
            else if (targetDir.magnitude < 0.2f)
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
        energy += Time.deltaTime * 2;
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
        Vector3 targetDir;
        if (distanceToPursuer.magnitude < 1)
        {
            targetDir = transform.position - pursuer.transform.position;

        }
        else
        {
            targetDir = home - transform.position;
        }

        velocity = Vector3.Lerp(velocity, targetDir.normalized * speed, Time.deltaTime * turnRadius);
        if (distanceToPursuer.magnitude > 1)
        {

            ChangeState(State.normal);

        }
    }

    protected void Search()
    {
        float xDir = Mathf.PerlinNoise(Time.time * -xOffset, Time.time + -yOffset) - 0.5f;
        float yDir = Mathf.PerlinNoise(Time.time * xOffset, Time.time + yOffset) - 0.5f;
        Vector3 moveDir = new Vector3(xDir, yDir, 0) * 2;
        velocity = Vector3.Lerp(velocity, moveDir * speed, Time.deltaTime);
    }

    protected void Patrol()
    {
        transform.RotateAround(home, Vector3.up, Time.deltaTime * 100);
        float xDir = Mathf.PerlinNoise(Time.time * -xOffset, Time.time + -yOffset) - 0.5f;
        float yDir = Mathf.PerlinNoise(Time.time * xOffset, Time.time + yOffset) - 0.5f;
        Vector3 moveDir = new Vector3(xDir, 0, yDir) * 10;
        velocity = Vector3.Lerp(velocity, moveDir * speed, Time.deltaTime);

    }
    
    protected void Defend()
    {
        if (curTarget != null)
        {
            Vector3 targetDir = (curTarget.transform.position - transform.position);
            if (targetDir.magnitude > 1)
            {
                targetDir.Normalize();

                
            }else if (targetDir.magnitude < 1)
            {
                if (curTarget.state != State.scared)
                {
                    curTarget.ChangeState(State.scared);
                }
            }
            else if (targetDir.magnitude < 0.2f)
            {
                
                targetDir.Normalize();
            }
            
            velocity = Vector3.Lerp(velocity, targetDir * speed, Time.deltaTime * turnRadius);
            

        }
        if (distanceToHome.magnitude > 2)
        {
            ChangeState(State.tired);
        }
    }

    protected void Attack()
    {
        //attacking is pursuing except the target is not caught, just killed
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
            
            case State.pursue:

                if (curTarget != null)
                {
                    StopChasing();
                }

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
            
            case State.pursue:

            
                break;
            
        }

        state = s;
    }

    private void OnDestroy()
    {
        if (captor != null)
        {
            if (captor.heldPrey.Contains(this))
            {
                
                captor.heldPrey.Remove(this);
            }else if (captor.capturedPrey.Contains(this))
            {
                captor.capturedPrey.Remove(this);
            }
        }

        if (curTarget != null)
        {
            StopChasing();
        }
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

                if (job == Job.defend)
                {
                    Patrol();
                    
                    if (FindClosestPredator())
                    {
                        ChangeState(State.pursue);
                    }

                }else if (job == Job.gather)
                {
                    Search();
                    
                    if (FindClosestTarget())
                    {
                        ChangeState(State.pursue);
                    }
                }
                
                if (energy < 0)
                {
                    ChangeState(State.tired);
                }
                
                break;
            
            case State.pursue :

                if (job == Job.gather)
                {
                    Pursue();
                    
                }else if (job == Job.defend)
                {
                    Defend();
                }

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

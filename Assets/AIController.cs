using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.AI;
public class AIController : CharacterSuper
{
    private FiniteStateMachine currentState;
    private NavMeshAgent agent;
    public GameObject player, spawn;
    
   
    public enum FiniteStateMachine
    {
      Idle, //used at round start and end to keep AI stationary.
      Defend,// if AI and Player is in AI base, kill player.
      Attack, //grabs AI flag at Enemy base, shoots player if within certain distance
      Capture, // grabs dropped flags within the field, will prefer this over attacking base
      Return //Has flag will return to base while attacking player if near
    }

    // Start is called before the first frame update
    void Start()
    {
         
         agent = gameObject.GetComponent<NavMeshAgent>();
         
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case FiniteStateMachine.Idle:
                isIdle();
                break;
        }
    }

    //applies the idle state
    public void isIdle()
    {
    }

    //applies the chasing state
    public void isChasing()
    {
        
        if (DistanceToPlayer(agent.transform.position.x, player.transform.position.x,agent.transform.position.y, player.transform.position.y ) > 2)
        {
            agent.SetDestination(player.gameObject.transform.position);
            
        }
        if (DistanceToPlayer(agent.transform.position.x, player.transform.position.x,agent.transform.position.y, player.transform.position.y ) <= 1.5f)
        {
            currentState = FiniteStateMachine.Attack;
        }
        
    }
//applies the attacking state
    public void isAttacking()
    {
        if (DistanceToPlayer(agent.transform.position.x, player.transform.position.x,agent.transform.position.y, player.transform.position.y ) > 1.4f)
        {
           //  currentState = FiniteStateMachine.Chase;
        }
    }

    //checks to see if player is within range and switches to chase mode
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
          //  currentState = FiniteStateMachine.Chase;
           // agent.SetDestination(player.gameObject.transform.position);
        }
    }
    
    //if player leaves switches to idle mode
    private void OnTriggerExit(Collider other)
    {
      //  currentState = FiniteStateMachine.Idle;
    }

    //used to calculate the distance to the player
    private float DistanceToPlayer(float x1, float x2, float y1, float y2)
    {
        float distance = (float)Math.Sqrt(Math.Pow((x2-x1),2) + Math.Pow((y2-y1),2)) ;
        return distance;
    }

    public override void Death()
    {
        gameObject.transform.position = spawn.transform.position;
        Health = MaxHealth;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "bullet")
        {
            Health -= Damage;
            Debug.Log("Shot");
            
        }

        if (Health <= 0)
        {
            Death();
        }
    }
}
using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.AI;
public class AIController : CharacterSuper
{
    private FiniteStateMachine currentState;
    private NavMeshAgent agent;
    public GameObject player, spawn,BulletSpawn, Bullet;
    private bool _canShoot = true, _inBase = true;
    private float timer = 5;
    public enum FiniteStateMachine
    {
      Idle, //used at round start and end to keep AI stationary.
      Defend,// if AI and Player is in AI base, kill player.
      Attack, //grabs AI flag at Enemy base, shoots player if within certain distance
      Capture, // grabs dropped flags within the field, will prefer this over attacking base
      Return, //Has flag will return to base while attacking player if near
      Heal
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = FiniteStateMachine.Idle;
         agent = gameObject.GetComponent<NavMeshAgent>();
         
    }
    
    // Update is called once per frame
    void Update()
    {
        
        Debug.Log(currentState);
        
        
        if (GameManager.Instance.Paused  )
        {
            currentState = FiniteStateMachine.Idle;
        }
        switch (currentState)
        {
            case FiniteStateMachine.Idle:
                idle();
                break;
            
            case FiniteStateMachine.Defend:
                defend();
                break;
            
            case FiniteStateMachine.Attack:
                break;
            
            case FiniteStateMachine.Capture:
                break;
            
            case FiniteStateMachine.Return:
                break;
            
            case FiniteStateMachine.Heal:
                break;
        }
    }

    public void idle()
    {
        agent.SetDestination(gameObject.transform.position);
    }
    
    public void defend()
    {
        //TODO
        //add distance on the x and z so he doesnt run into 
        if (DistanceToPlayer() > 3)
        {
           float random = Random.Range(-3, 3);
         agent.SetDestination(new Vector3(player.transform.position.x + random,player.transform.position.y,player.transform.position.z +random));
            
        }
        gameObject.transform.LookAt(player.transform.position);
        if (_canShoot)
        {
            StartCoroutine(shoot());
            _canShoot = false;
        }
    }


  
    private IEnumerator shoot()
    {
        GameObject spawnedBullet = Instantiate(Bullet,BulletSpawn.transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(-BulletSpawn.transform.forward*10, ForceMode.Impulse);
        yield return new WaitForSeconds(timer);
        _canShoot = true;
        Debug.Log("Shoot");

        yield return null;
    }
    
    
    
    
    
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Spawn_AI" && !GameManager.Instance.Paused)
        {
            _inBase = true;
            if (DistanceToObject(player.transform.position.x,transform.position.x,player.transform.position.z,transform.position.z) < 10)
            {
                currentState = FiniteStateMachine.Defend;
            }
        }
        
        if (other.gameObject.tag == "bullet")
        {
            Health -= Damage;
            Debug.Log("Shot");
            
        }
        Debug.Log("touch");
        
        if (other.gameObject.tag == "health")
        {
            Health = MaxHealth;

        }
        
        if (Health <= 0)
        {
            Death();
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spawn_AI")
        {
            _inBase = false;
            currentState = FiniteStateMachine.Attack;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Spawn_AI" && !GameManager.Instance.Paused )
        {
            _inBase = true;
            if (DistanceToObject(player.transform.position.x,transform.position.x,player.transform.position.z,transform.position.z) < 10)
            {
                currentState = FiniteStateMachine.Defend;
            }
        }    }

    //used to calculate the distance to the player
    private float DistanceToObject(float x1, float x2, float y1, float y2)
    {
        float distance = (float)Math.Sqrt(Math.Pow((x2-x1),2) + Math.Pow((y2-y1),2)) ;
        return distance;
    }
    
    
    //used to calculate the distance to the player
    private float DistanceToPlayer()
    {
        float distance = (float)Math.Sqrt(Math.Pow((transform.position.x-player.transform.position.x),2) + Math.Pow((transform.position.y-player.transform.position.y),2)) ;
        return distance;
    }

    public override void Death()
    {
        gameObject.transform.position = spawn.transform.position;
        Health = MaxHealth;
        
    }

 
    
    
}
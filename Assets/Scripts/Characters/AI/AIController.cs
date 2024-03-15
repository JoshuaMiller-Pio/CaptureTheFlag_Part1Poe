using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.AI;
public class AIController : CharacterSuper
{
    #region Declarations

        private FiniteStateMachine currentState;
        private NavMeshAgent agent;
        public GameObject player, spawn,BulletSpawn, Bullet;
        private bool _canShoot = true, _inBase = true, _enemyinBase = false ,_ismoving;

        private float timer = 2;
        
    #endregion
    
    
    
    
    
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
        player.GetComponent<PlayerController>().inEnemyBase += enemyCheck;
        currentState = FiniteStateMachine.Idle;
         agent = gameObject.GetComponent<NavMeshAgent>();
         
    }
    
    // Update is called once per frame
    void Update()
    {
      
        
        
        if (agent.hasPath)
        {
            _ismoving = true;
        }
        else
        {
            _ismoving = false;
        }
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
        if (_enemyinBase && _inBase)
        {
            if (DistanceToPlayer() > 3)
            {
                Debug.Log("moving");
                move();
                
            }
            
            //looks at player
            gameObject.transform.LookAt(player.transform.position);
            //if reload time has been satisfied allows shooting
            if (_canShoot)
            {
                StartCoroutine(shoot());
                _canShoot = false;

            }

        }
        
      //  if (!_enemyinBase  || !_inBase)
       // {
      //      currentState = FiniteStateMachine.Attack;
     //   }
        
        
        
        
        /*
        if (DistanceToPlayer() > 3.5f && DistanceToPlayer() < 10f)
        {
            move();
            if ((timer  - 1) ==0)
            {
                move();
            }
            
        }
        else if(DistanceToPlayer() > 10 && )
        {
            
        }
        gameObject.transform.LookAt(player.transform.position);
        if (_canShoot)
        {
            StartCoroutine(shoot());
            _canShoot = false;
        }*/
    }



    private void enemyCheck(object sender, EventArgs e)
    {
        if (_enemyinBase) _enemyinBase = false;
        else _enemyinBase = true;
    }
    private void move()
    {
        float randomx = Random.Range(-3.5f, 3.5f);
        float randomz = Random.Range(-3.5f, 3.5f);
        agent.SetDestination(new Vector3(player.transform.position.x ,player.transform.position.y,player.transform.position.z ));
    }
    
  
    private IEnumerator shoot()
    {
        GameObject spawnedBullet = Instantiate(Bullet,BulletSpawn.transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(-BulletSpawn.transform.forward*50, ForceMode.Impulse);
        yield return new WaitForSeconds(timer);
        _canShoot = true;

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
            
        }
        
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

    //used to calculate the distance to an object
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
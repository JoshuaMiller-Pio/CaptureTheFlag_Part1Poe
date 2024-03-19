using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.AI;
public class AIController : CharacterSuper
{
    #region Declarations

        private FiniteStateMachine _currentState,_previousState;
        private NavMeshAgent _agent;
        public GameObject player, spawn,BulletSpawn, Bullet ,flagB,flagR;
        private Vector3 _healPos;
        private GameObject[] _healthitems;
        private bool _canShoot = true, _inBase = true, _enemyinBase = false ,_ismoving;

        private float _timer = 2;
        
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
        player.GetComponent<PlayerController>().inEnemyBase += EnemyCheck;
        _currentState = FiniteStateMachine.Attack;
         _agent = gameObject.GetComponent<NavMeshAgent>();
         GameManager.Instance.RestartRound += RestartRound;
         _healthitems = GameObject.FindGameObjectsWithTag("health");
         

    }
    
    // Update is called once per frame
    void Update()
    {
        Debug.Log(_currentState);
        Debug.Log(_previousState);
      
        
        
        if (_agent.remainingDistance>0.1f)
        {
            _ismoving = true;
        }
        else
        {
            _ismoving = false;
        }
        
        /*
        if (GameManager.Instance.Paused  )
        {
            _currentState = FiniteStateMachine.Idle;
            _previousState = _currentState;
        }
        else if (_currentState == FiniteStateMachine.Idle && !GameManager.Instance.Paused)
        {
            _currentState = _previousState;
        }*/
        switch (_currentState)
        {
            case FiniteStateMachine.Idle:
                Idle();
                break;
            
            case FiniteStateMachine.Defend:
                Defend();
                break;
            
            case FiniteStateMachine.Attack:
                Attack();
                break;
            
            case FiniteStateMachine.Capture:
                break;
            
            case FiniteStateMachine.Return:
                Return();
                break;
            
            case FiniteStateMachine.Heal:
                Heal();
                break;
        }
    }

    
    
    
    
    
    public void Idle()
    {
        _agent.SetDestination(gameObject.transform.position);
    }
    
    private void Defend()
    {
        
        if (_enemyinBase && _inBase)
        {
            if (DistanceToPlayer() > 3 && !_ismoving)
            {
                Move();
                
            }
            Shootcheck();
            healthCheck();

        }
        

    }

    private void Attack()
    {
        _agent.SetDestination(flagR.transform.position);
        
        if (DistanceToPlayer() < 5)
        {
            Shootcheck();
        }
        healthCheck();
        //if picked up switch to return

    }
    private void Return()
    {
        _agent.SetDestination(spawn.transform.position);
        
        if (DistanceToPlayer() < 5)
        {
            Shootcheck();
        }
        healthCheck();
    }
    
    private void Heal()
    {
        _agent.SetDestination(spawn.transform.position);
        
        if (DistanceToPlayer() < 5)
        {
            Shootcheck();
            _agent.SetDestination(_healPos);
        }
        healthCheck();
    }


  
    private void Move()
    {
        float randomx = Random.Range(-3.5f, 3.5f);
        float randomz = Random.Range(-3.5f, 3.5f);
       // agent.SetDestination(new Vector3(player.transform.position.x ,player.transform.position.y,player.transform.position.z ));
    }

   
  
    private IEnumerator Shoot()
    {
        GameObject spawnedBullet = Instantiate(Bullet,BulletSpawn.transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(-BulletSpawn.transform.forward*50, ForceMode.Impulse);
        yield return new WaitForSeconds(_timer);
        _canShoot = true;

        yield return null;
    }






    #region triggers

        private void OnTriggerEnter(Collider other)
        {
            //takes damage on collision with bullet
            if (other.gameObject.tag == "bullet")
            {
                damage();
            }
            //if AI is in spawn and game isnt pause it goes into defense
            if (other.tag == "Spawn_AI" && !GameManager.Instance.Paused)
            {
                _inBase = true;
                if (DistanceToPlayer() < 5)
                {
                    _previousState = _currentState;
                    _currentState = FiniteStateMachine.Defend;
                    
                }
                else
                {
                    _previousState = _currentState;
                    _currentState = FiniteStateMachine.Attack;
                }
            }

            //if walks into flag, goes into return
            if (other.gameObject == flagR || other.gameObject == flagB)
            {
                _previousState = _currentState;
                _currentState = FiniteStateMachine.Return;
            }
            
            //on health pick up increases health and goes back to previous job
            if (other.gameObject.tag == "health")
            {
                Health = MaxHealth;
                //sets current state back to what it was previously
                _currentState = _previousState;
            }
            
           
        }

        private void OnTriggerExit(Collider other)
        {
            //on exit of spawn goes into attack mode
            if (other.tag == "Spawn_AI")
            {
                _inBase = false;
                _previousState = _currentState;
                _currentState = FiniteStateMachine.Attack;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Spawn_AI" && !GameManager.Instance.Paused )
            {
                _inBase = true;
                if (DistanceToPlayer() < 5)
                {
                    _previousState = _currentState;
                    _currentState = FiniteStateMachine.Defend;
                }
            }
            
        }


    #endregion
    

    #region calculations & checks
            
        private void EnemyCheck(object sender, EventArgs e)
        {
            if (_enemyinBase) _enemyinBase = false;
            else _enemyinBase = true;
        }

        private void healthCheck()
        {
            if (Health < MaxHealth)
            {
                for (int i = 0; i < _healthitems.Length; i++)
                {
                    if (DistanceToObject(_healthitems[i].transform.position.x, _healthitems[i].transform.position.z) < 5)
                    {
                        _previousState = _currentState;
                        _currentState = FiniteStateMachine.Heal;
                        _healPos=_healthitems[i].transform.position;
                    }

                    
                    
                }    
            }
            
        }
    
        private void Shootcheck()
        {
            //looks at player
            gameObject.transform.LookAt(player.transform.position);
            //if reload time has been satisfied allows shooting
            if (_canShoot)
            {
                StartCoroutine(Shoot());
                _canShoot = false;

            }
        }
        
        //used to calculate the distance to the player
        private float DistanceToPlayer()
        {
            float distance = (float)Math.Sqrt(Math.Pow((transform.position.x-player.transform.position.x),2) + Math.Pow((transform.position.y-player.transform.position.y),2)) ;
            return distance;
        }
        
        //used to calculate the distance to an object
        private float DistanceToObject( float x2,  float z2)
        {
            float distance = (float)Math.Sqrt(Math.Pow((x2-gameObject.transform.position.x),2) + Math.Pow((z2-gameObject.transform.position.z),2)) ;
            return distance;
        }
    

    #endregion

    public override void Death()
    {
        Flagdropped?.Invoke(this, EventArgs.Empty);
        gameObject.transform.position = spawn.transform.position;
        Health = MaxHealth;
        
    }

}
using System;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.AI;
public class AIController : CharacterSuper
{
    #region Declarations

        private FiniteStateMachine _currentState,_previousState;
        private NavMeshAgent _agent;
        public GameObject player, spawn,BulletSpawn, Bullet ,flagB,flagR;
        public GameObject shootlocation; 
        private Vector3 _healPos;
        private GameObject[] _healthitems;
        private bool _canShoot = true, _inBase = true, _enemyinBase = false ,_ismoving, _hasFlag = false;
        private Animator _anim;
        private float _timer = 2,_shootDistance = 10;
        
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
        GetComponent<InventoryScript>().aiFlagEquipt += hasFlag;
         _agent = gameObject.GetComponent<NavMeshAgent>();
        _currentState = FiniteStateMachine.Idle;
         _healthitems = GameObject.FindGameObjectsWithTag("health");
         _anim = gameObject.GetComponent<Animator>();
         GameManager.Instance.StartGame += startGame;
         GameManager.Instance.GameOver += gameover;
         


    }

  

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_currentState);
       
        
        
        if (_agent.remainingDistance>0.1f)
        {
            _ismoving = true;
        }
        else
        {
            _ismoving = false;
        }
        
        
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
                Capture();
                break;
            
            case FiniteStateMachine.Return:
                Return();
                break;
            
            case FiniteStateMachine.Heal:
                Heal();
                break;
        }
    }


    private void hasFlag(object sender, EventArgs e)
    {
        if (_hasFlag) _hasFlag = false;
        else _hasFlag = true;
        if (_currentState != FiniteStateMachine.Return)
        {
            _previousState = _currentState;
            _currentState = FiniteStateMachine.Return;
        }
        
    }    
    
    
    public void Idle()
    {
        _agent.SetDestination(gameObject.transform.position);
        setRunningfalse();
    }
    
    private void Defend()
    {
        
        if (_enemyinBase && _inBase)
        {
            if (DistanceToPlayer() > 3 && !_ismoving)
            {
                Move();
                
            }

            if (DistanceToPlayer() < _shootDistance)
            {
                Shootcheck();
            } 
            healthCheck();

        }
        else if(_currentState != FiniteStateMachine.Attack)
        {
            _previousState = _currentState;
            _currentState = FiniteStateMachine.Attack;
        }
        

    }

    private void Attack()
    {
        
        //sets destination to flag
        _agent.SetDestination(flagR.transform.position);
        setRunningtrue();
        if (DistanceToObject(flagB.transform.position.x, flagB.transform.position.z) <=30 && !GameManager.Instance.blueatBase )
        {
            _previousState = _currentState;
            _currentState = FiniteStateMachine.Capture;
        }
        //checks distance to player before shooting
        if (DistanceToPlayer() < _shootDistance )
        {
            Shootcheck();
        }
        healthCheck();
        

    }


    private void Capture()
    {
        //set destination to the flag in the field
        if (!GameManager.Instance.blueatBase)
        {
            _agent.SetDestination(flagB.transform.position);
        }
        //if flag is at base or distance is greater than 8 switch to attack
        if (DistanceToObject(flagB.transform.position.x, flagB.transform.position.z) > 30 || GameManager.Instance.blueatBase)
        {
            if (_currentState != FiniteStateMachine.Attack)
            { 
                _previousState = _currentState;
            _currentState = FiniteStateMachine.Attack;
                
            }
           
        }
        
        //shooting and health check
        if (DistanceToPlayer() < _shootDistance)
        {
            Shootcheck();
        }   
        healthCheck();

    }
    private void Return()
    {
        _agent.SetDestination(spawn.transform.position);
        setRunningtrue();
        if (_agent.remainingDistance <= 0 && _agent.remainingDistance > -50)
        {
            if (_currentState != FiniteStateMachine.Attack)
            {
                _previousState = _currentState;
                _currentState = FiniteStateMachine.Attack;
            }
        }
        if (DistanceToPlayer() < _shootDistance)
        {
            Shootcheck();
        }
        healthCheck();
    }
    
    private void Heal()
    {
        _agent.SetDestination(_healPos);
        setRunningtrue();

        if (DistanceToPlayer() < _shootDistance)
        {
            Shootcheck();
        }
        healthCheck();
    }


  
    private void Move()
    {
        setRunningtrue();
        float randomx = Random.Range(-3.5f, 3.5f);
        float randomz = Random.Range(-3.5f, 3.5f);
        _agent.SetDestination(new Vector3(player.transform.position.x ,player.transform.position.y,player.transform.position.z ));
    }

   
  
    private IEnumerator Shoot()
    {
        GameObject spawnedBullet = Instantiate(Bullet,BulletSpawn.transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(-BulletSpawn.transform.forward*50, ForceMode.Impulse);
        yield return new WaitForSeconds(_timer);
        _canShoot = true;

        yield return null;
    }

        private void startGame(object sender, EventArgs e)
        {
            _currentState = FiniteStateMachine.Attack;
        }
        private void gameover(object sender, EventArgs e)
        {
            _currentState = FiniteStateMachine.Idle;
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
            if (other.tag == "Spawn_AI" && _enemyinBase)
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



    #endregion
    

    #region calculations & checks
            
        //checks if player is in the AI base
        private void EnemyCheck(object sender, EventArgs e)
        {
            if (_enemyinBase) _enemyinBase = false;
            else _enemyinBase = true;
        }

        //checks distance to the nearest health item
        private void healthCheck()
        {
            float healthDist;
            if (Health < MaxHealth/2.4f)
            {
                for (int i = 0; i < _healthitems.Length; i++)
                {
                    healthDist = DistanceToObject(_healthitems[i].transform.position.x,
                        _healthitems[i].transform.position.z);
                    
                    if (healthDist< 20)
                    {
                        if (_currentState != FiniteStateMachine.Heal)
                        {
                            _previousState = _currentState;
                            _currentState = FiniteStateMachine.Heal;
                        }
                        _healPos=_healthitems[i].transform.position;
                    }

                    
                    
                }    
            }
            
        }
    
        private void Shootcheck()
        {
            //looks at player
            gameObject.transform.LookAt(shootlocation.transform.position);
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
            float distance = (float)Math.Sqrt(Math.Pow((transform.position.x-player.transform.position.x),2) + Math.Pow((transform.position.z-player.transform.position.z),2)) ;

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
        if (_currentState == FiniteStateMachine.Heal || _currentState == FiniteStateMachine.Return)
        {
            
            _currentState = FiniteStateMachine.Attack;
        }
        Health = MaxHealth;
        
    }


    private void setRunningfalse()
    {
       
            _anim.SetBool("isRunning", false);    
       
        
    }
    private void setRunningtrue()
    {
       
            _anim.SetBool("isRunning", true);    
       
        
    }

}
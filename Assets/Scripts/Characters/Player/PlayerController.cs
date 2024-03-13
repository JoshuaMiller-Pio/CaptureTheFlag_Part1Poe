using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : CharacterSuper
{
    #region Declarations

        [SerializeField] private CharacterController _controller;
        [SerializeField] private float speed = 13;
        [SerializeField]private LayerMask ground;
        [SerializeField]private GameObject spawn,BulletSpawn, Bullet;
        [SerializeField]private TextMeshProUGUI _healthDisplay;
        
        private Vector3 _velocity;
        private bool _grounded;
        public float timer = 1;
        public float gravity = -0f;
        private bool canShoot ;
    #endregion

    private void Start()
    {
        canShoot = true;
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward *z;
        _controller.Move(move * (speed * Time.deltaTime));

        _grounded = Physics.Raycast(gameObject.transform.position, -Vector3.up, 1, ground);
        if (_grounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);


        if (Input.GetAxis("Jump") == 1 && _grounded)
        {
            _velocity.y = 5;
        }

      

        if (Input.GetAxis("Fire1") >0 && canShoot)
        {
            StartCoroutine(shoot());
            canShoot = false;
        }
    }


    private IEnumerator shoot()
    {
         GameObject spawnedBullet = Instantiate(Bullet,BulletSpawn.transform.position, Quaternion.identity);
         spawnedBullet.GetComponent<Rigidbody>().AddForce(-BulletSpawn.transform.forward*50, ForceMode.Impulse);
             yield return new WaitForSeconds(timer);
         canShoot = true;


        yield return null;
    }
    
    public override void Death()
    {
        _controller.enabled = false;
        gameObject.transform.position = spawn.transform.position;
        
        Health = MaxHealth;

        _healthDisplay.text = Health.ToString();
        _controller.enabled = true;

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bullet")
        {
            Health -= Damage;
            _healthDisplay.text = Health.ToString();

        }
        if (other.gameObject.tag == "health")
        {
            Health = MaxHealth;
            _healthDisplay.text = Health.ToString();

        }

        if (Health <= 0)
        {
            Death();
        }
    }
}

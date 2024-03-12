using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float speed = 13;
    public float gravity = -0f;
    public LayerMask ground;
    private Vector3 _velocity;
    private bool grounded;
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward *z;
        _controller.Move(move * (speed * Time.deltaTime));

        grounded = Physics.Raycast(gameObject.transform.position, -Vector3.up, 1, ground);
        if (grounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);


        if (Input.GetAxis("Jump") == 1 && grounded)
        {
            _velocity.y = 5;
        }
      
    }


    /*
    [SerializeField] private float moveSpeed = 7f, lookSpeed = 2f,jumpheight= 6;
    private Rigidbody _rb;
    private Vector3 defaultVel , movement;
    private Coroutine Cmovement;

    private Vector2 _rotation;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rotation = Vector2.zero;
        defaultVel.y = _rb.velocity.y;
    }

    void Update()
    {
        
        StartCoroutine("MouseMovement");
        _rb.velocity = movement;
    }

    IEnumerator MouseMovement()
    {
        _rotation.y += Input.GetAxis("Mouse X");
        _rotation.x += -Input.GetAxis("Mouse Y");

        _rotation.x = Mathf.Clamp(_rotation.x, -30f, 30f);

        //look sides
        transform.eulerAngles = new Vector2(0, _rotation.y) * lookSpeed;

        //look up
        Camera.main.transform.localRotation = Quaternion.Euler(_rotation.x * lookSpeed, 0, 0);

        yield return null;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.action.name);
            Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
                gameObject.transform.position.z);

           if (Physics.Raycast(position, -Vector3.up, 1, 1 << 6))
            {
                
                _rb.AddForce(Vector3.up * jumpheight, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("not on ground");
            }
        }
    }

    private void FixedUpdate()
    {
    }

    public void Movement(InputAction.CallbackContext context)
    {
           if (context.started)
         {
             Cmovement =   StartCoroutine(MovePlayer(context));

         }
         else if (context.canceled)
         {
             StopCoroutine(Cmovement);
             _rb.velocity = defaultVel;
         }

    }
     private IEnumerator MovePlayer(InputAction.CallbackContext context)
     {
         while (true)
         {
             if (Input.GetAxis("Fire3") != 0)
             {
                 moveSpeed = 15;
             }
             else
             {
                 moveSpeed = 5;
             }

             Vector2 inputValue = context.ReadValue<Vector2>();


             Vector3 movementZ = transform.forward * inputValue.y;
             Vector3 movementX = transform.right * inputValue.x ;
             Vector3 movementY = transform.up * defaultVel.y;

             movement = movementX + movementZ  ;
             _rb.velocity = movement.normalized * moveSpeed ;
             Debug.Log("Running");
             yield return null; // Wait for the next frame
         }
     }
         */
}

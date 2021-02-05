using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] LayerMask layer;
    Animator animator;
    private float turn = 0;
    private bool isBlocking = false;
    public int Health = 100;
    public float Stamina = 100;

    //finds animator
    private void Awake() => animator = GetComponent<Animator>();

    //sets starting health
    private void Start()
    {
        Health = 100;
    }

    /*----
     * calls all movement functions
     * takes player input for movment, block and attack
     * manages stamina 
     * pass data to animator 
     */
    void Update()
    {
        if (Health > 0)
            aimMouse();
        sprint();
        block();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if (Input.GetMouseButtonDown(0) && Stamina > 20)
        {
            isBlocking = false;
            animator.SetTrigger("attack");
            Stamina = Stamina - 20;
        }

        if (Stamina < 15)
            isBlocking = false;

        if(Health >0)
            moving(movement);

        if (isBlocking == false && Stamina < 100)
            Stamina = Stamina + 0.01f;

        //Sets animation
        animator.SetFloat("VelocityX", movement.normalized.x, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityZ", movement.normalized.z, 0.1f, Time.deltaTime);
        animator.SetFloat("run", (speed / 5) - 1, 0.1f, Time.deltaTime);
        animator.SetFloat("turning", turn, 0.1f, Time.deltaTime);
        animator.SetFloat("health", Health);
        animator.SetBool("blocking", isBlocking);
    }

    //rotates character based on mouse movement
    void aimMouse()
    {
        turn = 0;
        if (Input.GetAxis("Mouse X") < 0)
        {
            this.transform.Rotate(0f, -0.5f, 0f, Space.Self);
            turn = 1;
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            this.transform.Rotate(0f, 0.5f, 0f, Space.Self);
            turn = 1;
        }
    }

    //increases speed while sprinting
    void sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isBlocking == false)
            speed = 10f;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = 5f;
    }

    //decreases speed and blocks with shield
    void block()
    {
        if (Input.GetMouseButtonDown(1))
        {    
            isBlocking = true; 
            speed = 2f;
        }

        if (Input.GetMouseButtonUp(1))
        { 
            isBlocking = false; 
            speed = 5f;
        }
    }

    //move character
    void moving(Vector3 movement)
    {
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= speed * Time.deltaTime;
            transform.Translate(movement, Space.Self);
        }
    }


    //checks if been hit, reduces health if hit, and stamina and health if hit while blocking
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "swordAtk")
        {
            if (Health > 0)
            {
                if (isBlocking == true)
                {
                    Health = Health - 5;
                    Stamina = Stamina - 5;
                }
                else
                    Health = Health - 20;
            }
            animator.SetTrigger("hit");
        }
    }
}

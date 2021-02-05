using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knightEvil : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    [SerializeField] LayerMask layer;
    Animator animator;
    private float turn = 0;
    private bool isBlocking = false;
    Vector3 locationTemp = new Vector3(0f, 0f, 0f);
    Vector3 location;
    float distance;
    int rand, rand2, rand3 = 0;
    int change, change2, rand6 = 0;
    public int Health = 100;
    private bool respawn = true;
    private prefabs paladin;

    /*----
     * Works the same as the player script
     * movement, attacks and block is randomized 
     * data is passed to the animator
     */
    private void Awake() => animator = GetComponent<Animator>();

    private void Start()
    {
        Debug.Log(Health);
        paladin = GameObject.Find("spawn").GetComponent<prefabs>();
        Health = 100;
    }

    void Update()
    {
        location = GameObject.Find("paladin_prop_j_nordstrom").transform.position;
        distance = Vector3.Distance(this.transform.position, location);
        if (Health > 0)
            aimMouse();
        sprint();
        block();
        if (distance > 2)
            isBlocking = false;

        rand = Random.Range(-1, 2);
        if (change < 500)
        {
            change++;
        }
        else
        {
            rand2 = rand;
            rand3 = Random.Range(0, 5);
            change = 0;
        }
        Vector3 movement = new Vector3(transform.position.x - location.x, 0f,transform.position.z - location.z);
        if (distance > 3)
            movement = new Vector3(0, 0, 1);
        if (distance < 1.3)
        {
            movement = new Vector3(rand2, 0, 0);
            speed = 2f;
        }
        if (rand3 == 1 && distance < 2.5)
        {
            movement = new Vector3(rand2, 0, -1);
            speed = 2f;
        }
        if (distance < 1.2)
            movement = new Vector3(0, 0, -1);


        float healthKnight = (GameObject.Find("paladin_prop_j_nordstrom").GetComponent<move>()).Health;
        if (healthKnight < 0.1)
        {
            animator.SetTrigger("win");
            movement = new Vector3(0, 0, 0);
        }
        
        if (Health > 0)
            moving(movement);
        animator.SetFloat("VelocityX", movement.normalized.x, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityZ", movement.normalized.z, 0.1f, Time.deltaTime);
        animator.SetFloat("run", (speed / 5) - 1, 0.1f, Time.deltaTime);
        animator.SetFloat("turning", turn, 0.1f, Time.deltaTime);
        animator.SetBool("blocking", isBlocking);
        attack();
        animator.SetFloat("health", Health);

        if (Health < 0.1 && respawn == true)
        {
            Instantiate(paladin.enemy);
            respawn = false;
        }
    }

    void aimMouse()
    {   
       //transform.forward = direction * Time.deltaTime*speed;
        if (location != locationTemp)
            turn = 1;
        else
           turn = 0;
        
        this.transform.LookAt(new Vector3(location.x, 0f, location.z));
        locationTemp = location;
        
    }

    void sprint()
    {
        if (distance > 7)
            speed = 9f;

        if (distance < 10)
            speed = 4f;
    }

    void block()
    {
        int rand5  = Random.Range(0, 5);
        if (Input.GetMouseButtonDown(0) && rand5 < 5)
            isBlocking = true;

        if (change2 < 1000)
        {
            change2++;
        }
        else
        {
            if (rand5 < 4)
                isBlocking = true;
            else
                isBlocking = false;
            
            change2 = 0;
        }    

        if (isBlocking == true)
            speed = 2f;
    }

    void moving(Vector3 movement)
    {
        
            if (movement.magnitude > 0)
            {
                movement.Normalize();
                movement *= speed * Time.deltaTime;
                transform.Translate(movement, Space.Self);
            }
        
    }

    void attack()
    {
        rand6 = Random.Range(0, 500);
        if (rand6 == 0 && distance < 1.8)
            animator.SetTrigger("attack");
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "swordAtk2")
        {
            if (Health > 0)
            {
                if (isBlocking == true)
                    Health = Health - 10;
                else
                    Health = Health - 30;
            }
            animator.SetTrigger("hit");  
        }
    }
}
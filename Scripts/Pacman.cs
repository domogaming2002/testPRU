using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public float speed1;   
    public Movement movement { get; private set; }
     

    public GameManager gameManager;
    public Animator animator;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        speed1 = movement.speed;
    }
    
    public void ResetState()
    {
        enabled = true;
        movement.ResetState();
        gameObject.SetActive(true);
        animator.SetBool("isDeath", false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameIsRunning)
        {
            movement.speed = 0f;
            movement.SetDirection(Vector2.left);
            float angle1 = Mathf.Atan2(-movement.direction.y, -movement.direction.x);
            transform.rotation = Quaternion.AngleAxis(angle1 * Mathf.Rad2Deg, Vector3.forward);
            return;
        }
        movement.speed = speed1;
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.movement.SetDirection(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.movement.SetDirection(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.movement.SetDirection(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.movement.SetDirection(Vector2.right);
        }

        float angle = Mathf.Atan2(-movement.direction.y, -movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void Death()
    {
        //set Animator
        animator.SetBool("isDeath", true);
    }
}

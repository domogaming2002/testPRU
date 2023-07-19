using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{  
    public float speed = 5f;            //Bien speed xac dinh toc do di chuyen 
    /*public float speedMultiplier = 1.0f;*/
   
    public Vector2 initialDirection;    //Vector xac dinh huong di chuyen ban dau 
   
    public LayerMask obstacleLayer;     //Xac dinh lop vat can
    public Rigidbody2D rigidBody { get; private set; }

    public Vector2 direction { get; private set; }          //Vector xac dinh huong di chuyen hien tai 

    public Vector2 nextDirection { get; private set; }      //Vector xac dinh huong di chuyen tiep theo

    public Vector3 startingPosition { get; private set; }   //Lay vi tri ban dau  


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    //Dat lai vi tri ban dau 
    public void ResetState()
    {
        //Debug.Log("reset in movement.cs");
        /*speedMultiplier = 1.0f;*/
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidBody.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        /*if (GameObject.Find("GameManager").GetComponent<GameManager>().gameIsRunning)
        {
            return;
        }??????????*/
        if (nextDirection != Vector2.zero)
        {
            
            SetDirection(nextDirection);
        }
    }

    //Ham di chuyen voi 1 toc do nhat dinh 
    private void FixedUpdate()
    {
        Vector2 position = rigidBody.position;      //Lay vi tri hien tai    
        Vector2 translation = direction * speed /** speedMultiplier*/ * Time.fixedDeltaTime;   
        rigidBody.MovePosition(position + translation); //Di chuyen den vi tri moi 
    }

    //Phuong thuc dat huong di chuyen 
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }


    //Phuong thuc kiem tra xem co vat can trong huong di chuyen hay khong
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;    //Neu co vat can tra ve true, nguoc lai la false
    }
}

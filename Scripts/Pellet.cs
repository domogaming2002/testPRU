using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public GameManager gameManager;
    public int point = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
            //StartCoroutine(gameManager.PelletEaten(this));
        }
    }

    private void Eat()
    {
        //StartCoroutine(FindObjectOfType<GameManager>().PelletEaten(this));
        FindObjectOfType<GameManager>().PelletEaten(this);
    }
}

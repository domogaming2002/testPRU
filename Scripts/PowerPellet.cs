using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Pellet
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
        //Debug.Log("test power eaten");
    }
}

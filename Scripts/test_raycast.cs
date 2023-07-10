using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_raycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D[] hitsDown;
        //shoot raycast going down
        hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down);
        Debug.Log(hitsDown.Length);
         
        //loop through all of the gameObjects that the raycast hits
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

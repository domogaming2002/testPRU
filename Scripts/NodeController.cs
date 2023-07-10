using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public bool canMoveLeft = false;
 
     public bool canMoveRight = false;
 
     public bool canMoveUp = false;
 
     public bool canMoveDown = false;
 
     public GameObject nodeLeft;
 
     public GameObject nodeRight;
 
     public GameObject nodeUp;
 
     public GameObject nodeDown;
     // Start is called before the first frame update
     void Start()
     {
         RaycastHit2D[] hitsDown;
         //shoot raycast going down
         hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up);
         
         //loop through all of the gameObjects that the raycast hits
         for (var i = 0; i < hitsDown.Length; i++)
         {
             var distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
             if (!(distance < 1.1f)) continue;
             canMoveDown = true;
             nodeDown = hitsDown[i].collider.gameObject;
         }
     }
 
     // Update is called once per frame
     void Update()
     {
         
     }
 }

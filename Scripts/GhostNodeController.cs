using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostNodeController : MonoBehaviour
{
    public bool canMoveLeft = false;
 
    public bool canMoveRight = false;
 
    public bool canMoveUp = false;
 
    public bool canMoveDown = false;
 
    public GameObject nodeLeft;
 
    public GameObject nodeRight;
 
    public GameObject nodeUp;
 
    public GameObject nodeDown;

    public bool isWarpRightNode = false;
    
    public bool isWarpLeftNode = false;

    public bool isGhostStartingNode = false;

    public bool isSideNode = false;
    // Start is called before the first frame update
    void Start()
    {
        //shoot raycast going down
        var hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down);

        //loop through all of the gameObjects that the raycast hits
        for (var i = 0; i < hitsDown.Length; i++)
        {
            var distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
            if (distance < 1f && hitsDown[i].collider.tag == "GhostNode")
            {
                canMoveDown = true;
                nodeDown = hitsDown[i].collider.gameObject;
                break;
            }
        }
        
        //shoot raycast going up
        var hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up);

        //loop through all of the gameObjects that the raycast hits
        for (var i = 0; i < hitsUp.Length; i++)
        {
            var distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
            if (distance < 1f && hitsUp[i].collider.tag == "GhostNode")
            {
                canMoveUp = true;
                nodeUp = hitsUp[i].collider.gameObject;
                break;
            }
        }
        
        //shoot raycast going down
        var hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right);

        //loop through all of the gameObjects that the raycast hits
        for (var i = 0; i < hitsRight.Length; i++)
        {
            var distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
            if (distance < 1f && hitsRight[i].collider.tag == "GhostNode")
            {
                canMoveRight = true;
                nodeRight = hitsRight[i].collider.gameObject;
                break;
            }
        }
        
        //shoot raycast going down
        var hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left);

        //loop through all of the gameObjects that the raycast hits
        for (var i = 0; i < hitsLeft.Length; i++)
        {
            var distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
            if (distance < 1f && hitsLeft[i].collider.tag == "GhostNode")
            {
                canMoveLeft = true;
                nodeLeft = hitsLeft[i].collider.gameObject;
                break;
            }
        }
        
    }
 
    void Update()
    // Update is called once per frame
    {
        
    }

    public GameObject GetNodeFromDirection(string direction)
    {
        return direction switch
        {
            "left" when canMoveLeft => nodeLeft,
            "right" when canMoveRight => nodeRight,
            "down" when canMoveDown => nodeDown,
            "up" when canMoveUp => nodeUp,
            _ => null
        };
    }
}

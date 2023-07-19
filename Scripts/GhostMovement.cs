using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject currentNode;

    public float speed = 1f;

    public string direction = "";

    public string lastMovementDirection = "";

    public bool canWarp = true;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameIsRunning)
        {
            return;
        }
        var currentNodeController = currentNode.GetComponent<GhostNodeController>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);
        
        var reverseDirection = (direction == "left" && lastMovementDirection == "right")
                                ||(direction == "right" && lastMovementDirection == "left")
                                ||(direction == "up" && lastMovementDirection == "down")
                                ||(direction == "down" && lastMovementDirection == "up");

        if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y)
        {
            GetComponent<EnemyController>().ReachedCenterOfNode(currentNodeController);
            if (currentNodeController.isWarpLeftNode && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovementDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            else if (currentNodeController.isWarpRightNode && canWarp)
            {
                currentNode = gameManager.leftWarpNode;
                direction = "right";
                lastMovementDirection = "right";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            else
            {
                //if we are not a ghost that is respawning , and we are on the start node, and we are trying to move down, stop
                if (currentNodeController.isGhostStartingNode 
                    && direction == "down" 
                    && (GetComponent<EnemyController>().ghostNodesState != EnemyController.GhostNodeStatesEnum.respawning))
                {
                    direction = lastMovementDirection;
                }
                var newNode = currentNodeController.GetNodeFromDirection(direction);
                if (newNode != null)
                {
                    currentNode = newNode;
                    lastMovementDirection = direction;
                }
                else
                {
                    direction = lastMovementDirection;
                    newNode = currentNodeController.GetNodeFromDirection(direction);
                    if (newNode != null)
                    {
                        currentNode = newNode;
                    }
                }
            }
        }
        else
        {
            canWarp = true;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetDirection(string direct)
    {
        lastMovementDirection = direction;
        direction = direct;
    }
}

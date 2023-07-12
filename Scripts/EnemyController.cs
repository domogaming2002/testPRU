using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum GhostNodeStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStatesEnum ghostNodesState;
    public GhostNodeStatesEnum respawnState;

    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostType ghostType;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public GhostMovement movementController;

    public GameObject startingNode;
    public bool readyToLeaveHome = false;
    public GameManager gameManager;

    public bool testRespawn = false;

    public bool isFrightened = false;

    public GameObject[] scatterNodes;
    public int scatterNodeIndex;

    public bool isVisible = true;

    public SpriteRenderer ghostSprite;
    public SpriteRenderer eyesSprite;

    public Animator animator;

    public Color color;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ghostSprite = GetComponent<SpriteRenderer>();
        eyesSprite = GetComponentInChildren<SpriteRenderer>();

        scatterNodeIndex = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<GhostMovement>();
        if (ghostType == GhostType.red)
        {
            ghostNodesState = GhostNodeStatesEnum.startNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodesState = GhostNodeStatesEnum.leftNode;
            respawnState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodesState = GhostNodeStatesEnum.rightNode;
            respawnState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodesState = GhostNodeStatesEnum.centerNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
        }

        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    public void SetUp()
    {
        animator.SetBool("moving", false);
    }

    
    

    // Update is called once per frame
    void Update()
    {
        //Show our sprites
        if (isVisible)
        {
            if (ghostNodesState != GhostNodeStatesEnum.respawning)
            {
                ghostSprite.enabled = true;
            }
            eyesSprite.enabled = true;
        } else
        {
            ghostSprite.enabled = false;
            eyesSprite.enabled = false;
        }


        if (isFrightened)
        {
            animator.SetBool("frightened", true);
            //eyesSprite.enabled = false;
            ghostSprite.color = new Color(255, 255, 255, 255);
        } else
        {
            animator.SetBool("frightened", false);
            ghostSprite.color = color;

        }

        animator.SetBool("moving", true);


        if (testRespawn == true)
        {
            readyToLeaveHome = false;
            ghostNodesState = GhostNodeStatesEnum.respawning;
            testRespawn = false;
        }

        if (movementController.currentNode.GetComponent<GhostNodeController>().isSideNode)
        {
            movementController.SetSpeed(2);
        }
        else
        {
            movementController.SetSpeed(4);
        }
    }

    public void ReachedCenterOfNode(GhostNodeController nodeController)
    {
        if (ghostNodesState == GhostNodeStatesEnum.movingInNodes)
        {
            //Scatter mode
            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {
                DetermineGhostScatterDirection();
            }
            //Frightened mode
            else if(isFrightened)
            {
                if (!nodeController.isSideNode)
                {
                    var direction = GetRandomDirection();
                    movementController.SetDirection(direction);
                }
                
                //Debug.Log(direction);
            }
            //Chase mode
            else
            {
                //determine next node to go to
                if (ghostType == GhostType.red)
                {
                    DetermineRedGhostDirection();
                }
                else if (ghostType == GhostType.pink)
                {
                    DeterminePinkGhostDirection();
                }
                else if (ghostType == GhostType.blue)
                {
                    DetermineBlueGhostDirection();
                }
                else if (ghostType == GhostType.orange)
                {
                    DetermineOrangeGhostDirection();
                }
            }
        }
        else if (ghostNodesState == GhostNodeStatesEnum.respawning)
        {
            var direction = "";
            //we have reached our start node, move to the center node
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
                //Debug.Log("good");
            }
            //we have reached our center node either finish respawn or move to the left/right node
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if (respawnState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodesState = respawnState;
                }
                else if (respawnState == GhostNodeStatesEnum.leftNode)
                {
                    direction = "left";
                }
                else if(respawnState == GhostNodeStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }
            //if our respawn state is either the left or right node, and we got to that node, leave home again
            else if (
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
                || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y))
            {
                ghostNodesState = respawnState;
            }
            //we are in the game board still, locate our start node
            else
            {
                //Debug.Log("lonnnnn");
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }
            //determine quickest direction to home
            //Debug.Log(direction);
            movementController.SetDirection(direction);
        }
        else
        {
            //if we are ready to leave our home
            if (readyToLeaveHome)
            {
                if (ghostNodesState is GhostNodeStatesEnum.leftNode)
                {
                    ghostNodesState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                else if (ghostNodesState is GhostNodeStatesEnum.rightNode)
                {
                    ghostNodesState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("left");
                }
                else if (ghostNodesState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodesState = GhostNodeStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                else if (ghostNodesState == GhostNodeStatesEnum.startNode)
                {
                    ghostNodesState = GhostNodeStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }

    string GetRandomDirection()
    {
        var possibleDirections = new List<string>();
        var nodeController = movementController.currentNode.GetComponent<GhostNodeController>();
        if (nodeController.canMoveDown && movementController.lastMovementDirection != "up")
        {
            possibleDirections.Add("down");
            //Debug.Log("down");
        }
        if (nodeController.canMoveUp && movementController.lastMovementDirection != "down")
        {
            possibleDirections.Add("up");
            //Debug.Log("up");
        }
        if (nodeController.canMoveLeft && movementController.lastMovementDirection != "right")
        {
            possibleDirections.Add("left");
            //Debug.Log("left");
        }
        if (nodeController.canMoveRight && movementController.lastMovementDirection != "left")
        {
            possibleDirections.Add("right");
            //Debug.Log("right");
        }

        if (possibleDirections.Count > 0)
        {
            var direction = "";
            var randomDirectionIndex = Random.Range(0, possibleDirections.Count -1 );
            //Debug.Log(possibleDirections.Count);
            direction = possibleDirections[randomDirectionIndex];
            return direction;
        }

        return null;
    }
    void DetermineGhostScatterDirection()
    {
        //if we reached the scatter node, add one to our scatter node index
        if (transform.position.x == scatterNodes[scatterNodeIndex].transform.position.x
            && transform.position.y == scatterNodes[scatterNodeIndex].transform.position.y)
        {
            scatterNodeIndex++;
            if (scatterNodeIndex == scatterNodes.Length)
                scatterNodeIndex = 0;
        }
        var direction = GetClosestDirection(scatterNodes[scatterNodeIndex].transform.position);
        movementController.SetDirection(direction);
    }
    void DetermineRedGhostDirection()
    {
        var direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    void DeterminePinkGhostDirection()
    {
        var pacmanDirection = gameManager.pacman.GetComponent<Movement>().direction;
        var distanceBetweenNode = 1f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmanDirection == Vector2.left)
        {
            target.x -= (distanceBetweenNode * 4);
        }
        else if (pacmanDirection == Vector2.right)
        {
            target.x += (distanceBetweenNode * 4);
        }
        else if (pacmanDirection == Vector2.down)
        {
            target.y -= (distanceBetweenNode * 4);
        }
        else if (pacmanDirection == Vector2.up)
        {
            target.y += (distanceBetweenNode * 4);
        }

        var direction = GetClosestDirection(target);
        movementController.SetDirection(direction);
    }

    void DetermineBlueGhostDirection()
    {
        var pacmanDirection = gameManager.pacman.GetComponent<Movement>().direction;
        var distanceBetweenNode = 1f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmanDirection == Vector2.left)
        {
            target.x -= (distanceBetweenNode * 2);
        }
        else if (pacmanDirection == Vector2.right)
        {
            target.x += (distanceBetweenNode * 2);
        }
        else if (pacmanDirection == Vector2.down)
        {
            target.y -= (distanceBetweenNode * 2);
        }
        else if (pacmanDirection == Vector2.up)
        {
            target.y += (distanceBetweenNode * 2);
        }

        var redGhost = gameManager.redGhost;
        var xDistance = target.x - redGhost.transform.position.x;
        var yDistance = target.y - redGhost.transform.position.y;

        var blueTarget = new Vector2(target.x + xDistance, target.y + yDistance);
        var direction = GetClosestDirection(blueTarget);
        movementController.SetDirection(direction);
    }

    void DetermineOrangeGhostDirection()
    {
        var distance = Vector2.Distance(gameManager.pacman.transform.position, transform.position);
        var distanceBetweenNode = 1f;
        distance = distance < 0 ? distance * -1 : distance;
        // if we are within 8 nodes of pacman
        if (distance <= distanceBetweenNode * 8)
        {
            DetermineRedGhostDirection();
        }
        //otherwise 
        else
        {
            //scatter mode
            DetermineGhostScatterDirection();
        }
    }

    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        var lastMovingDirection = movementController.lastMovementDirection;
        var newDirection = "";

        var nodeController = movementController.currentNode.GetComponent<GhostNodeController>();
        
        //if we can move up and we aren't reversing
        if (nodeController.canMoveUp && lastMovingDirection != "down")
        {
            var nodeUp = nodeController.nodeUp;
            var distance = Vector2.Distance(nodeUp.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";
            }
        }
        
        //if we can move down and we aren't reversing
        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            var nodeDown = nodeController.nodeDown;
            var distance = Vector2.Distance(nodeDown.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }
        }
        
        //if we can move right and we aren't reversing
        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {
            var nodeRight = nodeController.nodeRight;
            var distance = Vector2.Distance(nodeRight.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";
            }
        }
        
        //if we can move left and we aren't reversing
        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            var nodeLeft = nodeController.nodeLeft;
            var distance = Vector2.Distance(nodeLeft.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";
            }
        }

        return newDirection;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public Ghost[] ghosts;
    //public Pacman pacman

    public Transform pellets;
    public int score { get; set; }

    public Text txtScore;
    public Text txtLevel;

    public GameObject live1, live2, live3;
    public int lives { get; set; }
    public int currentLevel;
    
    public GameObject leftWarpNode;

    public GameObject rightWarpNode;

    public GameObject pacman;
    

    
    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public GameObject redGhost;
    public GameObject pinkGhost;
    public GameObject blueGhost;
    public GameObject orangeGhost;

    public EnemyController redGhostController;
    public EnemyController pinkGhostController;
    public EnemyController blueGhostController;
    public EnemyController orangeGhostController;

    public int totalPellets = 0;
    public int pelletsLeft = 0;
    public int pelletedCollectedOnThisLife = 0;

    public bool hadDeathOnThisLevel = false;
    public bool gameIsRunning;
    
    public bool isPowerPelletRunning = false;
    public float currentPowerPelletTime = 0;
    public float powerPelletTimer = 8f;
    public int powerPelletMultiplyer = 1;
    public enum GhostMode
    {
        chase, scatter
    }

    public GhostMode currentGhostMode;

    public bool newGame;
    public bool clearedLevel;

    public Image blackBackground;

    //public int[6][5] ghostModeTimers =  {{ 7, 20, 7, 20, 5},{7,20,7,20,5}};
    public int[,] ghostModetimers = new int[6, 5]
    {
        { 7, 20, 7, 20, 5 },
        { 7, 20, 7, 20, 5 },
        { 5, 20, 5, 20, 5 },
        { 5, 30, 5, 30, 5 },
        { 3, 30, 3, 30, 3 },
        { 3, 30, 3, 30, 3 }
    };
    public int ghostModeTimerIndex;
    public float ghostModeTimer = 0;
    public bool completedTimer;
    public bool runningTimer;

    // Start is called before the first frame update
    private void Update()
    {
        switch (this.lives){ 
            case 0:
                live1.gameObject.SetActive(false);
                live2.gameObject.SetActive(false);
                live3.gameObject.SetActive(false);
                break;
            case 1:
                live1.gameObject.SetActive(true);
                live2.gameObject.SetActive(false);
                live3.gameObject.SetActive(false);
                break;
            case 2:
                live1.gameObject.SetActive(true);
                live2.gameObject.SetActive(true);
                live3.gameObject.SetActive(false);
                break;
            default:
                live1.gameObject.SetActive(true);
                live2.gameObject.SetActive(true);
                live3.gameObject.SetActive(true);
                break;

        }
        if (!gameIsRunning)
        {
            return;
        }
        if(!completedTimer && runningTimer)
        {
            ghostModeTimer += Time.deltaTime;
            if (currentLevel > 6)
            {
                completedTimer = true;
                runningTimer = false;
                currentGhostMode = GhostMode.chase;
            }
            else if (ghostModeTimer >= ghostModetimers[currentLevel-1, ghostModeTimerIndex])
            {
                ghostModeTimer = 0;
                ghostModeTimerIndex++;
                if (currentGhostMode == GhostMode.chase)
                {
                    currentGhostMode = GhostMode.scatter;
                }
                else
                {
                    currentGhostMode = GhostMode.chase;
                }

                if (ghostModeTimerIndex == 6)
                {
                    completedTimer = true;
                    runningTimer = false;
                    currentGhostMode = GhostMode.chase;
                }
            }
        }

        if (isPowerPelletRunning)
        {
            currentPowerPelletTime += Time.deltaTime;
            if (currentPowerPelletTime >= powerPelletTimer)
            {
                isPowerPelletRunning = false;
                currentPowerPelletTime = 0;
                powerPelletMultiplyer = 1;
            }
        }
    }

    void Awake()
    {
        newGame = true;
        clearedLevel = true;
        blackBackground.enabled = false;
        redGhostController = redGhost.GetComponent<EnemyController>();
        pinkGhostController = pinkGhost.GetComponent<EnemyController>();
        blueGhostController = blueGhost.GetComponent<EnemyController>();
        orangeGhostController = orangeGhost.GetComponent<EnemyController>();
        ghostNodeStart.GetComponent<GhostNodeController>().isGhostStartingNode = true;

        pacman = GameObject.Find("Pacman");
        //txtLevel.text = "Level: " + currentLevel.ToString();
        //Setup();
        StartCoroutine(Setup());
    }

    public IEnumerator Setup()
    {
        ghostModeTimerIndex = 0;
        ghostModeTimer = 0;
        completedTimer = false;
        runningTimer = true;
        //Debug.Log("hi setup " + clearedLevel);
        if (clearedLevel)
        {
            //Debug.Log("helllo");
            blackBackground.enabled = true;
            yield return new WaitForSeconds(1);
        }

        blackBackground.enabled = false;
        pelletedCollectedOnThisLife = 0;
        currentGhostMode = GhostMode.scatter;
        gameIsRunning = false;

        var waitTimer = 1f;
        
        if (clearedLevel || newGame)
        {
            waitTimer = 3;
            NewRound();
            GetTotalPellets();
        }

        if (newGame)
        {
            NewGame();
        }
        txtLevel.text = "Level: " + currentLevel.ToString();
        pacman.GetComponent<Pacman>().ResetState();
        redGhostController.SetUp();
        pinkGhostController.SetUp();
        blueGhostController.SetUp();
        orangeGhostController.SetUp();

        newGame = false;
        clearedLevel = false;
        yield return new WaitForSeconds(waitTimer);
        blackBackground.enabled = false;
        StartGame();
    }

    void StartGame()
    {
        gameIsRunning = true;
    }

    void StopGame()
    {
        gameIsRunning = false;
    }
    void GetTotalPellets()
    {
        totalPellets = 0;
        pelletsLeft = 0;
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                totalPellets++;
                pelletsLeft++;
            }
        }

        //Debug.Log("total pellet = " + totalPellets);
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        currentLevel = 1;
    }

    private void NewRound()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
    }

    private void SetLives(int live)
    {
        this.lives = live;
       
    }

    public void SetScore(int score)
    {
        this.score = score;
        txtScore.text = "Score: " + score.ToString();
        //Debug.Log("Diem hien tai:" + score);
    }
    

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        pelletsLeft--;
        pelletedCollectedOnThisLife++;
        var requiredBluePellets = 0;
        var requiredOrangePellets = 0;

        if (hadDeathOnThisLevel)
        {
            requiredBluePellets = 12;
            requiredOrangePellets = 32;
        }
        else
        {
            requiredBluePellets = 30;
            requiredOrangePellets = 60;
        }

        if (pelletedCollectedOnThisLife >= requiredBluePellets && !blueGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            blueGhost.GetComponent<EnemyController>().readyToLeaveHome = true;
        }
        if (pelletedCollectedOnThisLife >= requiredOrangePellets && !orangeGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            orangeGhost.GetComponent<EnemyController>().readyToLeaveHome = true;
        }

        SetScore(this.score + pellet.point);

        //check if there are any pellet left
        if (pelletsLeft <= 0)
        {
            //Debug.Log("totalPellet: " + totalPellets);
            //Debug.Log("pelletleft: " + pelletsLeft);
            currentLevel++;
            clearedLevel = true;
            StopGame();
            //Setup();
            //Debug.Log("before wait");
            //yield return new WaitForSeconds(1);
            //Debug.Log("after wait " + clearedLevel);
            StartCoroutine(Setup());
            //yield return Setup();
            //Invoke(nameof(NewRound), 3.0f);
            //Debug.Log("End game");
        }
        
        //
    } 
    
    public void PowerPelletEaten(PowerPellet pellet)
    {
        PelletEaten(pellet);
        //Debug.Log("test power eaten in game manager");
        //thay doi trang thai cua ma
        isPowerPelletRunning = true;
        currentPowerPelletTime = 0;
        

        redGhostController.SetFrightened(true);
        blueGhostController.SetFrightened(true);
        orangeGhostController.SetFrightened(true);
        pinkGhostController.SetFrightened(true);
    }

    public IEnumerator PauseGame(float timeToPause)
    {
        gameIsRunning = false;
        yield return new WaitForSeconds(timeToPause);
        gameIsRunning = true;
    }
    public void GhostEaten()
    {
        SetScore(score + 400 * powerPelletMultiplyer);
        powerPelletMultiplyer++;
        StartCoroutine(PauseGame(1));
    }
    public IEnumerator PlayerEaten()
    {
        hadDeathOnThisLevel = true;
        pacman.GetComponent<Pacman>().Death();
        StopGame();
        yield return new WaitForSeconds(1);
        lives--;
        if (lives <= 0)
        {
            newGame = true;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("GameOverScene 1");
            //Display game over text
        }
        //Setup();
        StartCoroutine(Setup());
    }
    
    
    
}

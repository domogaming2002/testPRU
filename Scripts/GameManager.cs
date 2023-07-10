using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public Ghost[] ghosts;
    //public Pacman pacman

    public Transform pellets;
    public int score { get; set; }
    public int lives { get; set; }
    
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

    public int totalPellets = 0;
    public int pelletsLeft = 0;
    public int pelletedCollectedOnThisLife = 0;
    public enum GhostMode
    {
        chase, scatter
    }

    public GhostMode currentGhostMode;
    

    // Start is called before the first frame update
    void Awake()
    {
        GetTotalPellets();
        pinkGhost.GetComponent<EnemyController>().readyToLeaveHome = true;
        currentGhostMode = GhostMode.chase;
        NewGame();
    }

    void GetTotalPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                totalPellets++;
            }
        }

        Debug.Log("total pellet = " + totalPellets);
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
        //Chua co ma va pacman
    }

    private void ResetState()
    {


        //Chua co ma va pacman (Gamemanager vid 3 tieng)
    }

    public void GameOver()
    {

    }

    private void SetLives(int live)
    {
        this.lives = live;
    }

    public void SetScore(int score)
    {
        this.score = score;
        Debug.Log("Diem hien tai:" + score);
    }

    public void GhostEaten()
    {

    }
    public void PacManEaten()
    {
        SetLives(this.lives - 1);
        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        totalPellets++;
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.point);

        if (!HasRemainingPellets())
        {

            //Invoke(nameof(NewRound), 3.0f);
            Debug.Log("End game");
        }
    } 
    
    public void PowerPelletEaten(PowerPellet pellet)
    {
        PelletEaten(pellet);
        
        //thjay doi trang thai cua ma
    }

    public bool HasRemainingPellets()
    {
        var pelletCount = 0;
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                Debug.Log("hayyyyyyyy");
                pelletCount++;
            }
        }

        pelletsLeft = pelletCount;
        return pelletCount > 0;
    }
}

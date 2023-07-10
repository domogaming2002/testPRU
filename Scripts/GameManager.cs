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


    // Start is called before the first frame update
    void Start()
    {
        NewGame();
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
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}

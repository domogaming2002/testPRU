using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    // public Boolean isGamePaused = false;
    public GameObject pauseGameUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyUp(KeyCode.Space))
        {
            // if (isGamePaused)
            // {
            //     Resume();
            // }
            // else
            // {
            //     Pause();
            // }
            Pause();
        }        
    }
    public void Pause()
    {
        pauseGameUI.SetActive(true); //active panel
        // Time.timeScale = 0f;
        // isGamePaused = true;
    }

    // public void Resume()
    // {
    //     pauseGameUI.SetActive(false); //inactice panel
    //     // Time.timeScale = 1f;
    //     isGamePaused = false;
    // }

}

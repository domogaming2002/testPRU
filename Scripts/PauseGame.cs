using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public Canvas PauseCanvas;
    public Button buttonResume;
    public Button buttonQuit;
    // Start is called before the first frame update
    void Start()
    {
        PauseCanvas.enabled = false;
        buttonResume.onClick.AddListener(Resume);
        buttonQuit.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            PauseCanvas.enabled = true;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume() {
        Time.timeScale = 1;
        PauseCanvas.enabled = false;
    }

    public void Quit() {
        SceneManager.LoadScene("StartMenu");
    }
}

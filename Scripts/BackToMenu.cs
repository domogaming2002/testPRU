using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    public Button buttonBack;
    // Start is called before the first frame update
    void Start()
    {
        buttonBack.onClick.AddListener(changeScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeScene()
    {
        SceneManager.LoadScene("StartMenu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    //public GameManager gameManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressPauseButton();
        }
    }

    public void PressPauseButton()
    {
        if (Time.timeScale == 1)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        //gameManager.StartMusic();
        Time.timeScale = 1;
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        //gameManager.PauseMusic();
        Time.timeScale = 0;
    }

    public void Quit(){
        Application.Quit();
    }
}

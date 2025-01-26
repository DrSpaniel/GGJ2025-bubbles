using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        GameManager.gamestate = GameManager.GameState.Playing;
        //gameManager.StartMusic();
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        GameManager.gamestate = GameManager.GameState.Paused;
        //gameManager.PauseMusic();
    }

    public void Restart(){
        pauseMenuUI.SetActive(false);
        GameManager.gamestate = GameManager.GameState.Restart;
    }

    public void Quit(){
        Application.Quit();
    }
}

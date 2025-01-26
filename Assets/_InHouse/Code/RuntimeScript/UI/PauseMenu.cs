using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
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
    public void LoadMainMenu()
    {
        pauseMenuUI.SetActive(false);
        GameManager.gamestate = GameManager.GameState.MainMenu;
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        GameManager.gamestate = GameManager.GameState.Playing;
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        GameManager.gamestate = GameManager.GameState.Paused;
    }

    public void Restart(){
        pauseMenuUI.SetActive(false);
        GameManager.gamestate = GameManager.GameState.Restart;
    }

    public void Quit(){
        Application.Quit();
    }
}

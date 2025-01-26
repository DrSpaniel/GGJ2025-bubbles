using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Make an instance of this class
    private static GameManager instance;

    public static GameManager Instance
    { get { return instance; } }

    public enum GameState
    {
        MainMenu = 0,
        Playing = 1,
        Paused = 2,
        GameOver = 3,
        Restart = 4,
        Exit = 5
    }

    public static GameState gamestate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamestate = GameState.Playing;

    }

    // Update is called once per frame
    void Update()
    {
        switch (gamestate)
        {
            case GameState.MainMenu:
                SceneManager.LoadScene("StartScene");
                GameManager.gamestate = GameManager.GameState.Playing;
                break;
            case GameState.Playing:
                Time.timeScale = 1;
                // Playing logic
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                // Paused logic
                break;
            case GameState.GameOver:
                // Game over logic
                Time.timeScale = 0;
                // reset? load new scene? etc
                break;
            case GameState.Restart:
                GameManager.gamestate = GameManager.GameState.Playing;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case GameState.Exit:
                Application.Quit();
                break;
        }

    }
}
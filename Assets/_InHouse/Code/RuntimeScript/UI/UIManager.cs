using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public HealthBarUI healthBarUI;
    public PauseMenu pauseMenu;
    public GameObject endGameUI;

    private AudioSource m_AudioSource;
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.PressPauseButton();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CloseEndGameAndReset();
        }
    }

    public void ShowEndGameUI()
    {
        endGameUI.SetActive(true);
    }

    public void CloseEndGameAndReset() {
        endGameUI.SetActive(false);
        pauseMenu.pauseMenuUI.SetActive(false);
        GameManager.gamestate = GameManager.GameState.Restart;
    }

    private void OnEnable()
    {
        EventManager.OnHealthChanged += HandleHealthChanged;
        EventManager.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        EventManager.OnHealthChanged -= HandleHealthChanged;
        EventManager.OnDeath -= HandleDeath;
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth){
        healthBarUI.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void HandleDeath(){
        m_AudioSource.Play();
        ShowEndGameUI();
        GameManager.gamestate = GameManager.GameState.Paused;
    }
}

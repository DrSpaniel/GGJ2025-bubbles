using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HealthBarUI healthBarUI;

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
        Debug.Log("UIManager: Character Died");
    }
}

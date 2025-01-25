using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar UI Elements")]
    public Image greenBarImage;   

    [Header("Health Settings")]
    public float maxHealth = 100f; 
    private float currentHealth;  

    void Start()
    {
        // initialize health
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 
        UpdateHealthBar();
    }

    [ContextMenu("Heal")]
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (greenBarImage != null)
        {
            greenBarImage.fillAmount = currentHealth / maxHealth;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Health Bar UI Elements")]
    public Image greenBarImage;   

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (greenBarImage != null)
        {
            greenBarImage.fillAmount = currentHealth / maxHealth;
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class BubbleHealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [SerializeField] private float defaultCollisionDamage = 20f;
    [SerializeField] private float soapHeals = 50f;
    [SerializeField] private float damageOverTime = 0.5f;
    private float currentHealth;

    void Start()
    {
        // initialize health
        currentHealth = maxHealth;
        //takeDamageOverTime
        StartCoroutine(ApplyDamageOverTime());
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetHealth();
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        StopAllCoroutines();
        StartCoroutine(ApplyDamageOverTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        int otherLayer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(otherLayer);

        switch (layerName)
        {
            case "Heal":
                Destroy(collision.gameObject);
                Heal(soapHeals);
                break;
            default:
                TakeDamage(defaultCollisionDamage);
                break;
        }
       return;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // broadcast health change event
        EventManager.BroadcastHealthChanged(currentHealth, maxHealth);

        if(currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // broadcast event
        EventManager.BroadcastHealthChanged(currentHealth, maxHealth);
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (currentHealth > 0)
        {
            TakeDamage(damageOverTime);
            yield return new WaitForSeconds(1f); 
        }
    }

    public void Die() {
        Debug.Log("Character Died");
        EventManager.BroadcastDeath();
        StopCoroutine(ApplyDamageOverTime());
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<float, float> OnHealthChanged;
    public static Action OnDeath;

    public static void BroadcastHealthChanged(float currentHealth, float maxHealth){
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public static void BroadcastDeath(){
        OnDeath?.Invoke();
    }

}

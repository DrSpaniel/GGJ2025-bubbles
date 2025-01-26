using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverPopNoise : MonoBehaviour
{

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        audioSource.Play();
    }
}

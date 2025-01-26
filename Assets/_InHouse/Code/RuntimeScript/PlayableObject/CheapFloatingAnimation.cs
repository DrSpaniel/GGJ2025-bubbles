using DG.Tweening;
using UnityEngine;

public class CheapFloatingAnimation : MonoBehaviour
{
    public float floatDistance = 0.5f;
    public float floatDuration = 2f;

    private void Start()
    {
        StartFloating();
    }

    private void StartFloating()
    {
        Vector3 startPosition = transform.position;

        transform.DOMoveY(startPosition.y + floatDistance, floatDuration)
        .SetEase(Ease.InOutSine)  
            .SetLoops(-1, LoopType.Yoyo); 
    }
}

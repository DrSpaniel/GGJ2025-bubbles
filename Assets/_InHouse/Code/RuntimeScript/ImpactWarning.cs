using UnityEngine;

public class ImpactWarning : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;

    [SerializeField] private float indicatorLifeTime = 2f;

    private void Start()
    {
        print("Script attached and running.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contactPoint in collision.contacts) {
            GameObject indicator = Instantiate(indicatorPrefab, contactPoint.point, Quaternion.identity);
            Destroy(indicator, indicatorLifeTime);
        }
    }
}

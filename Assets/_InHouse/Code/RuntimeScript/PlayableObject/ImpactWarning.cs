using UnityEngine;

public class ImpactWarning : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;

    [SerializeField] private float indicatorLifeTime = 1.5f;
    [SerializeField] private float cooldownTime = 0.5f;

    private float nextAllowedTime = 0f;

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time >= nextAllowedTime)
        {
            nextAllowedTime = Time.time + cooldownTime;
            foreach (ContactPoint contactPoint in collision.contacts)
            {
                GameObject indicator = Instantiate(indicatorPrefab, contactPoint.point, Quaternion.identity);
                Destroy(indicator, indicatorLifeTime);
            }
        }
    }
}

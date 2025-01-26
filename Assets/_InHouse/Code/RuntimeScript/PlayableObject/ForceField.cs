using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForceField : MonoBehaviour
{
    [Header("Force Settings")]
    public Vector3 forceDirection = Vector3.forward; 
    public float forceStrength = 0.01f;         

    [Header("Force Field Visualization")]
    public Color gizmoColor = new Color(0f, 1f, 1f, 0.3f); // 可视化颜色

    private Collider forceFieldCollider;

    private void Awake()
    {
        // 获取 Collider
        forceFieldCollider = GetComponent<Collider>();
        if (!forceFieldCollider.isTrigger)
        {
            Debug.LogWarning("ForceField's Collider should be set to 'Is Trigger' for proper behavior.");
            forceFieldCollider.isTrigger = true; // 自动设置为触发器
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 force = transform.TransformDirection(forceDirection.normalized) * forceStrength;
            rb.AddForce(force, ForceMode.Impulse);
            Debug.Log($"Applied force to {other.name}: {force}");
        }
    }

    private void OnDrawGizmos()
    {
        if (forceFieldCollider == null)
            forceFieldCollider = GetComponent<Collider>();

        if (forceFieldCollider != null)
        {
            Gizmos.color = gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix;

            if (forceFieldCollider is BoxCollider box)
            {
                Gizmos.DrawCube(box.center, box.size);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (forceFieldCollider == null)
            return;

        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
        Gizmos.matrix = transform.localToWorldMatrix;

        if (forceFieldCollider is BoxCollider box)
        {
            Gizmos.DrawWireCube(box.center, box.size);
        }
        else if (forceFieldCollider is SphereCollider sphere)
        {
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
        }
    }
}

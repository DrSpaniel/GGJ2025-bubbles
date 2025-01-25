using UnityEngine;

public class BubbleDeformer : MonoBehaviour
{
    [Header("Mesh Child Reference")]
    [SerializeField] private Transform bubbleMeshChild;

    [Header("Wind Source")]
    [SerializeField] private WindController windController;

    [Header("Deformation Settings")]
    [SerializeField] private float maxSquash = 0.90f;  // Along wind direction
    [SerializeField] private float maxStretch = 1.00f; // Perpendicular to wind
    [SerializeField] private float windStrengthFactor = 1f;

    // Stores the child's original scale
    private Vector3 childOriginalScale;

    private void Awake()
    {
        if (!bubbleMeshChild)
        {
            Debug.LogError("BubbleDeformer: Missing reference to the mesh child.");
            enabled = false;
            return;
        }
        childOriginalScale = bubbleMeshChild.localScale;
    }

    private void Update()
    {
        if (!windController) return;

        // Get wind data
        Vector3 windDir = windController.windDirection;
        float rawMagnitude = windController.windMagnitude;

        // Map magnitude (e.g., range 0-15) to 0-1
        float strength = Mathf.InverseLerp(0f, windController.maxStrength, rawMagnitude);
        float baseStrength = strength * windStrengthFactor;

        // 假设 strength 已经在 [0,1] 范围内
        float dynamicFrequency = 1f + (strength * 1f);
        // 当 strength=0 时，频率=1；当 strength=1 时，频率=4

        // 以 Time.time * speed 作为 Perlin Noise 的输入，可以得到平滑随机
        float noiseValue = Mathf.PerlinNoise(Time.time * 1.5f, 0f); // 1.5f 是速度可调
                                                                    // noiseValue 会在 [0,1] 内波动，我们让它中心在 0，再缩小范围
        float noiseOffset = (noiseValue - 0.5f) * dynamicFrequency; // 这里 0.2f 表示最大±0.1 的幅度

        strength = Mathf.Clamp01(baseStrength + noiseOffset);

        // If wind is too weak or direction is zero, reset to original
        if (strength < 0.01f || windDir.sqrMagnitude < 0.0001f)
        {
            ResetBubble();
            return;
        }

        // Calculate squash/stretch factors
        float squash = Mathf.Lerp(1f, maxSquash, strength);
        float stretch = Mathf.Lerp(1f, maxStretch, strength);

        // Rotate the child to face the wind
        bubbleMeshChild.rotation = Quaternion.LookRotation(windDir.normalized, Vector3.up);

        // Apply scale: Z = wind direction (squash), X/Y = perpendicular (stretch)
        bubbleMeshChild.localScale = new Vector3(
            childOriginalScale.x * stretch,
            childOriginalScale.y * stretch,
            childOriginalScale.z * squash
        );

        // Keep child at the parent's center
        bubbleMeshChild.localPosition = Vector3.zero;
    }

    private void ResetBubble()
    {
        bubbleMeshChild.localPosition = Vector3.zero;
        bubbleMeshChild.localRotation = Quaternion.identity;
        bubbleMeshChild.localScale = childOriginalScale;
    }
}

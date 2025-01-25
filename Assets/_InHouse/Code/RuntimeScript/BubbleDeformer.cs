using UnityEngine;

public class BubbleDeformer : MonoBehaviour
{
    [Header("Mesh Child Reference")]
    [SerializeField] private Transform bubbleMeshChild;

    [Header("Wind Source")]
    [SerializeField] private WindController windController;

    [Header("Deformation Settings")]
    [SerializeField] private float maxSquash = 0.95f;  // Along wind direction
    [SerializeField] private float maxStretch = 1.05f; // Perpendicular to wind
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

        // Add some dynamic frequency to the wind
        float dynamicFrequency = 1f + (strength * 1f);
        // Use Time.time * speed as input to Perlin Noise for smooth randomness
        float noiseValue = Mathf.PerlinNoise(Time.time * 1.5f, 0f); 
            // noiseValue will fluctuate within [0,1], center it around 0 and shrink the range
        float noiseOffset = (noiseValue - 0.5f) * dynamicFrequency; 

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

using UnityEngine;

public class WindController : MonoBehaviour
{
    public Vector3 windDirection;
    // Calculate the wind direction
    public float windMagnitude;
    [SerializeField] private bool debug = false;

    [Header("Target & Wind Settings")]
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private float maxDistance = 4f;
    public float maxStrength = 20f;

    [Header("Fallback Wind (Out of Range)")]
    [SerializeField] private Vector3 fallbackWindDirection = Vector3.up * 0.1f;
    [SerializeField] private float fallbackWindStrength = 1f;

    private Material _targetMaterial;
    private Camera _mainCamera;
    private Transform _targetTransform;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _targetTransform = targetGameObject.transform;
    }

    void Start()
    {
        _targetMaterial = targetGameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition(-0.1f);

        // Calculate distance Vector between target and mouse
        Vector3 toMouse = mouseWorldPosition - _targetTransform.position;
        float windDistance = toMouse.magnitude;

        if (windDistance > maxDistance)
        {
            // If not in range, use fallback values, we assume liquid fall down by gravity 
            windDirection = fallbackWindDirection;
            windMagnitude = fallbackWindStrength;

        }else{
            // other wise calculate the wind direction and strength
            windDirection = (mouseWorldPosition - targetGameObject.transform.position).normalized;
            // project the wind distance to a value between 0 and 1
            float t = windDistance / maxDistance;
            windMagnitude = Mathf.Lerp(maxStrength, 0f, t);
        }

        Vector3 windSpeed = windDirection * windMagnitude;

        // Update the material
        _targetMaterial.SetVector("_WindDirection", windDirection.normalized);
        _targetMaterial.SetFloat("_WindStrength", windSpeed.magnitude);


        // Apply force to the target object
        targetGameObject.transform.parent.GetComponent<Rigidbody>().AddForce(-windSpeed * .5f, ForceMode.Acceleration);

        if (debug)
        {
            Debug.Log(-windDirection.normalized * windSpeed.magnitude);
            Debug.DrawLine(targetGameObject.transform.position, targetGameObject.transform.position + windDirection * windMagnitude, Color.red);
        }
        if (windSpeed.magnitude > 0.01f)
        {
            Vector3 movementDirection = new Vector3(windDirection.normalized.x, 0, windDirection.normalized.z);
            Quaternion targetRotation = Quaternion.LookRotation(-movementDirection);
            targetGameObject.transform.parent.rotation = Quaternion.Slerp(targetGameObject.transform.parent.rotation, targetRotation, Time.deltaTime * 1f);
        }
    }

    // obtain mouse world position
    private Vector3 GetMouseWorldPosition(float planeIncrement){
        Vector3 mousePosition = Input.mousePosition;
        float objectDepth = _mainCamera.WorldToScreenPoint(targetGameObject.transform.position).z + planeIncrement;
        return _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, objectDepth));
    }
}

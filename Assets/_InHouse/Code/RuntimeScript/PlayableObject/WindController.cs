using UnityEngine;

public class WindController : MonoBehaviour
{
    public Vector3 windDirection;
    public float windMagnitude;
    public float planeIncrement = -0.1f;
    [SerializeField] private bool debug = false;

    [Header("Target & Wind Settings")]
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private GameObject windGameObject;
    [SerializeField] private float maxDistance = 4f;
    [SerializeField] private float maxObjectSpeed = 7f;
    public float maxStrength = 20f;

    [Header("Fallback Wind (Out of Range)")]
    [SerializeField] private Vector3 fallbackWindDirection = Vector3.up;
    [SerializeField] private float fallbackWindStrength = 1f;

    private Material _targetMaterial;
    private Camera _mainCamera;
    private Transform _targetTransform;
    private Rigidbody _targetRigidbody;
    // VFX/SFX
    private ParticleSystem _targetParticleSystem;
    private AudioSource _targetAudioSource;

    

    private void Awake()
    {
        _mainCamera = Camera.main;
        _targetTransform = targetGameObject.transform;
        _targetRigidbody = targetGameObject.transform.parent.GetComponent<Rigidbody>();
        _targetParticleSystem = windGameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        _targetAudioSource = windGameObject.transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Start()
    {
        _targetMaterial = targetGameObject.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        CalculateWindParameters(mouseWorldPosition);
        UpdateMaterialProperties();
        DebugWind(windDirection, windDirection * windMagnitude);
    }

    private void FixedUpdate()
    {
        if (windDirection != Vector3.zero)
        {
            //Update indicator for wind direction
            windGameObject.transform.position = GetMouseWorldPosition();
            Quaternion targetRotation = Quaternion.LookRotation(windDirection, Vector3.up);
            Quaternion adjustment = Quaternion.Euler(0, 180, 0);
            windGameObject.transform.rotation = targetRotation * adjustment;
            Vector3 cameraDirection = (Camera.main.transform.position - windGameObject.transform.position).normalized;
            float distance = 0.5f; // distance from camera
            windGameObject.transform.position += cameraDirection * distance;
        }
        if (Input.GetMouseButton(0))
        {
            if (!_targetParticleSystem.isPlaying)
            {
                _targetParticleSystem.Play();
            }
            if(!_targetAudioSource.isPlaying){
                _targetAudioSource.Play();
            }
            ApplyWindForce();
        }else{
            if (_targetParticleSystem.isPlaying)
            {
                _targetParticleSystem.Stop();
            }
            if (_targetAudioSource.isPlaying)
            {
                _targetAudioSource.Stop();
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        if (_mainCamera == null) return Vector3.zero; 

        Vector3 mousePosition = Input.mousePosition;
        float objectDepth = _mainCamera.WorldToScreenPoint(_targetTransform.position).z + planeIncrement;
        return _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, objectDepth));
    }

    private void CalculateWindParameters(Vector3 mouseWorldPosition)
    {
        Vector3 toMouse = mouseWorldPosition - _targetTransform.position;
        float windDistance = toMouse.magnitude;

        if (windDistance > maxDistance)
        {
            windDirection = fallbackWindDirection;
            windMagnitude = fallbackWindStrength;
        }
        else
        {
            windDirection = toMouse.normalized;
            float clampedDistance = Mathf.Clamp(windDistance, 0f, maxDistance);
            float t = clampedDistance / maxDistance;
            windMagnitude = Mathf.Lerp(maxStrength, 0f, t);
        }
    }

    private void ApplyWindForce()
    {
        Vector3 windSpeed = windDirection * windMagnitude;
        _targetRigidbody.AddForce(-windSpeed * 0.5f, ForceMode.Acceleration);

        if (_targetRigidbody.linearVelocity.magnitude > maxObjectSpeed)
        {
            _targetRigidbody.linearVelocity = _targetRigidbody.linearVelocity.normalized * maxObjectSpeed;
        }

        // rotate object towards wind direction
        if (windSpeed.magnitude > 0.01f)
        {
            Vector3 movementDirection = new Vector3(windDirection.x, 0, windDirection.z);
            if (movementDirection != Vector3.zero) { 
                Quaternion targetRotation = Quaternion.LookRotation(-movementDirection);
                _targetTransform.parent.rotation = Quaternion.Slerp(_targetTransform.parent.rotation, targetRotation, Time.deltaTime);
            }
        }

        // apply air resistance
        _targetRigidbody.AddForce(-_targetRigidbody.linearVelocity * 0.1f, ForceMode.Acceleration);
       }

    private void UpdateMaterialProperties()
    {
        if (!_targetMaterial.GetVector("_WindDirection").Equals(windDirection.normalized))
            _targetMaterial.SetVector("_WindDirection", windDirection.normalized);

        if (!_targetMaterial.GetFloat("_WindStrength").Equals(windMagnitude))
            _targetMaterial.SetFloat("_WindStrength", windMagnitude);
    }

    private void DebugWind(Vector3 windDirection, Vector3 windSpeed)
    {
        if (!debug) return;

        Debug.Log($"Wind Direction: {windDirection}, Wind Speed: {windSpeed.magnitude}");
        Debug.DrawLine(_targetTransform.position, _targetTransform.position + windDirection * windMagnitude, Color.red);
    }
}

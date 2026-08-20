using UnityEngine;

public class StrobeLightController : MonoBehaviour
{
    [Header("Spotlight Reference")]
    [SerializeField] private Light spotLight;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second
    [SerializeField] private float minXRotation = -90f; // Minimum X rotation
    [SerializeField] private float maxXRotation = 120f; // Maximum X rotation
    [SerializeField] private AnimationCurve rotationPattern = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    
    private float _currentRotationTime = 0f;
    private float _baseIntensity;
    private Vector3 _baseRotation;
    private float _stepInterval;

    private void Start()
    {
        if (spotLight == null)
        {
            spotLight = GetComponent<Light>();
        }
        _baseIntensity = spotLight.intensity;
        _baseRotation = transform.eulerAngles;
    }

    private void Update()
    {
        UpdateLightRotation();
    }

    private void UpdateLightRotation()
    {
        _currentRotationTime += Time.deltaTime;
        float rotationRange = Mathf.Abs(maxXRotation - minXRotation);
        float cycleTime = (_currentRotationTime * rotationSpeed) / (rotationRange * 2f);
        cycleTime = cycleTime % 1f; 
        float curveValue = rotationPattern.Evaluate(cycleTime);
        float xRotation = Mathf.Lerp(minXRotation, maxXRotation, curveValue);
        Vector3 newRotation = _baseRotation;
        newRotation.x = xRotation;
        transform.eulerAngles = newRotation;
    }
}
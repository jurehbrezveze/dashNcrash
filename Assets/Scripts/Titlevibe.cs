using UnityEngine;

public class TitleVibe : MonoBehaviour
{
    [Header("Scale Settings")]
    public float scaleAmount = 0.05f; // How much it scales up/down
    public float scaleSpeed = 2f; // Speed of the pulsing

    [Header("Rotation Settings")]
    public float rotationAmount = 5f; // Degrees to sway
    public float rotationSpeed = 1f; // Speed of swaying

    private Vector3 originalScale;
    private Quaternion originalRotation;

    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Smooth pulsing scale
        float scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        transform.localScale = originalScale + Vector3.one * scaleOffset;

        // Gentle rotation sway
        float rotationOffset = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, rotationOffset);
    }
}

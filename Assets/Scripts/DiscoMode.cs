using UnityEngine;

public class ColorCycleChildrenOnly : MonoBehaviour
{
    [Header("Color Cycle Settings")]
    [Range(0f, 1f)] public float saturation = 1f;
    [Range(0f, 1f)] public float value = 1f;
    public float cycleSpeed = 1f;

    private float hue;
    private SpriteRenderer[] childSprites;

    void Start()
    {
        // Get all SpriteRenderers in children
        childSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        hue += Time.deltaTime * cycleSpeed;
        hue %= 1f;

        Color currentColor = Color.HSVToRGB(hue, saturation, value);

        foreach (var sr in childSprites)
        {
            if (sr != null)
                sr.color = currentColor;
        }
    }
}

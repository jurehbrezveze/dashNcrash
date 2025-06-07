using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HeightColorPoint
{
    public float height;
    public Color color;
}

public class HeightColorChanger : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player;

    [Header("Objects To Change Color")]
    public List<Renderer> targetObjects;

    [Header("Color Points by Height")]
    public List<HeightColorPoint> colorPoints;

    [Header("Lerp Settings")]
    public float lerpSpeed = 2f;

    private Color currentColor;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not assigned.");
            enabled = false;
            return;
        }

        if (colorPoints == null || colorPoints.Count < 2)
        {
            Debug.LogError("You need at least two height-color points.");
            enabled = false;
            return;
        }

        // Sort the color points by height ascending
        colorPoints.Sort((a, b) => a.height.CompareTo(b.height));
        currentColor = colorPoints[0].color;
    }

    private void Update()
    {
        float y = player.position.y;

        HeightColorPoint lower = colorPoints[0];
        HeightColorPoint upper = colorPoints[colorPoints.Count - 1];

        for (int i = 0; i < colorPoints.Count - 1; i++)
        {
            if (y >= colorPoints[i].height && y <= colorPoints[i + 1].height)
            {
                lower = colorPoints[i];
                upper = colorPoints[i + 1];
                break;
            }
        }

        float t = Mathf.InverseLerp(lower.height, upper.height, y);
        Color targetColor = Color.Lerp(lower.color, upper.color, t);

        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpSpeed);

        foreach (Renderer r in targetObjects)
        {
            if (r != null && r.material.HasProperty("_Color"))
            {
                r.material.color = currentColor;
            }
        }
    }
}

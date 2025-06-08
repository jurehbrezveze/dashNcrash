using UnityEngine;

public class ColorByY : MonoBehaviour
{
    public Transform player;
    public float yScale = 10f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (player == null || rend == null) return;

        float hue = Mathf.Repeat(player.position.y / yScale, 1f);
        Color color = Color.HSVToRGB(hue, 1f, 1f);

        rend.material.color = color;
    }
}

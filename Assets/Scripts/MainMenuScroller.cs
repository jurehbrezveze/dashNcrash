using UnityEngine;

public class MainMenuScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Tooltip("Speed at which the camera moves upward")]
    public float scrollSpeed = 1f;

    [Tooltip("Enable or disable scrolling")]
    public bool scrollingEnabled = true;

    [Tooltip("Set a maximum Y position to stop scrolling (optional)")]
    public float maxYPosition = Mathf.Infinity;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("MainMenuScroller: No main camera found!");
        }
    }

    void Update()
    {
        if (scrollingEnabled && cam != null)
        {
            Vector3 newPos = cam.transform.position + new Vector3(0, scrollSpeed * Time.deltaTime, 0);

            // Optional max Y stop
            if (newPos.y <= maxYPosition)
            {
                cam.transform.position = newPos;
            }
        }
    }
}

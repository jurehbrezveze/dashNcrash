using UnityEngine;
using System.Collections.Generic;

public class ColorZoneController : MonoBehaviour
{
    [System.Serializable]
    public class ColorZone
    {
        public float minY;
        public float maxY;
        public Color objectColor;
        public Color cameraColor;
    }

    public Transform player;
    public string targetTag = "ColorZoneObj";
    public List<ColorZone> zones = new List<ColorZone>();
    public float transitionSpeed = 5f;

    private List<Renderer> targetRenderers = new List<Renderer>();
    private Color currentObjectColor;
    private Color targetObjectColor;
    private Color currentCameraColor;
    private Color targetCameraColor;
    private Camera mainCam;

    void Start()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in taggedObjects)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                targetRenderers.Add(rend);

                // Force material to opaque to avoid invisibility
                Material mat = rend.material;
                mat.SetFloat("_Mode", 0);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = -1;
            }
        }

        mainCam = Camera.main;

        // Get the starting zone
        float playerY = player != null ? player.position.y : 0f;
        foreach (ColorZone zone in zones)
        {
            if (playerY >= zone.minY && playerY < zone.maxY)
            {
                currentObjectColor = zone.objectColor;
                targetObjectColor = zone.objectColor;
                currentCameraColor = zone.cameraColor;
                targetCameraColor = zone.cameraColor;
                break;
            }
        }

        // Apply immediately
        foreach (Renderer rend in targetRenderers)
        {
            rend.material.color = currentObjectColor;
        }

        if (mainCam != null)
        {
            mainCam.backgroundColor = currentCameraColor;
        }
    }

    void Update()
    {
        if (player == null || zones.Count == 0) return;

        float playerY = player.position.y;

        foreach (ColorZone zone in zones)
        {
            if (playerY >= zone.minY && playerY < zone.maxY)
            {
                targetObjectColor = zone.objectColor;
                targetCameraColor = zone.cameraColor;
                break;
            }
        }

        // Smooth transitions only after Start
        currentObjectColor = Color.Lerp(currentObjectColor, targetObjectColor, Time.deltaTime * transitionSpeed);
        currentObjectColor.a = 1f;

        currentCameraColor = Color.Lerp(currentCameraColor, targetCameraColor, Time.deltaTime * transitionSpeed);

        foreach (Renderer rend in targetRenderers)
        {
            rend.material.color = currentObjectColor;
        }

        if (mainCam != null)
        {
            mainCam.backgroundColor = currentCameraColor;
        }
    }
}

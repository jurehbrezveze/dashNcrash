using UnityEngine;

public class GlitchTrigger2D : MonoBehaviour
{
    [Tooltip("Duration of the glitch flash effect in seconds")]
    public float glitchDuration = 0.2f;

    [Tooltip("Optional objects to reveal during glitch")]
    public GameObject[] revealObjects;

    [Tooltip("Optional audio to play when glitch happens")]
    public AudioSource glitchSound;

    private InvertEffect invertEffect;
    private bool hasTriggered = false;

    void Start()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            invertEffect = cam.GetComponent<InvertEffect>();
            if (invertEffect == null)
                Debug.LogError("InvertEffect not found on Main Camera!");
        }
        else
        {
            Debug.LogError("No Main Camera found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player") && invertEffect != null)
        {
            hasTriggered = true;

            if (glitchSound != null)
                glitchSound.Play();

            invertEffect.TriggerGlitch(glitchDuration);

            foreach (var obj in revealObjects)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            Destroy(gameObject); // Remove trigger after use
        }
    }
}

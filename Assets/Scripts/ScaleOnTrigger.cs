using UnityEngine;
using System.Collections;

public class ScaleOnTrigger : MonoBehaviour
{
    [Header("Timing")]
    public float scaleDuration = 0.5f;
    public float waitDuration = 3f;

    [Header("Scaling")]
    public Vector3 originalScale = Vector3.one;
    public Vector3 targetScale = Vector3.zero;

    [Header("Particle Effects")]
    public ParticleSystem[] particles;

    [Header("Tag Detection")]
    public string triggeringTag = "Player";

    private bool isTriggered = false;

    void Start()
    {
        // Store initial scale in case it's not set
        if (originalScale == Vector3.zero)
            originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag(triggeringTag))
        {
            isTriggered = true;
            StartCoroutine(HandleScaling());
        }
    }

    IEnumerator HandleScaling()
    {
        // Play particles
        foreach (var ps in particles)
        {
            if (ps != null) ps.Play();
        }

        // Scale down
        yield return StartCoroutine(ScaleObject(transform.localScale, targetScale, scaleDuration));

        // Wait
        yield return new WaitForSeconds(waitDuration);

        // Scale up
        yield return StartCoroutine(ScaleObject(transform.localScale, originalScale, scaleDuration));

        isTriggered = false; // allow retrigger
    }

    IEnumerator ScaleObject(Vector3 from, Vector3 to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
    }
}

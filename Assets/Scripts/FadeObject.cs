using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class FadeObject : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeOutDuration = 1f;
    public float fadeInDuration = 1f;
    public float invisibleDuration = 3f;

    [Header("Behavior Settings")]
    public bool fadeBackIn = true;
    public bool disableColliderWhenInvisible = true;
    public bool destroyAfterFadeOut = false;

    [Header("Collision Settings")]
    public string playerTag = "Player";

    private SpriteRenderer sr;
    private Collider2D col;
    private bool isFading = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFading && collision.gameObject.CompareTag(playerTag))
        {
            StartCoroutine(FadeRoutine());
        }
    }

    IEnumerator FadeRoutine()
    {
        isFading = true;

        // Fade out
        yield return StartCoroutine(FadeAlpha(1f, 0f, fadeOutDuration));

        if (disableColliderWhenInvisible)
            col.enabled = false;

        if (destroyAfterFadeOut)
        {
            Destroy(gameObject);
            yield break;
        }

        if (fadeBackIn)
        {
            yield return new WaitForSeconds(invisibleDuration);

            // Fade in
            yield return StartCoroutine(FadeAlpha(0f, 1f, fadeInDuration));

            if (disableColliderWhenInvisible)
                col.enabled = true;
        }

        isFading = false;
    }

    IEnumerator FadeAlpha(float start, float end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, timer / duration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(end);
    }

    void SetAlpha(float alpha)
    {
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}

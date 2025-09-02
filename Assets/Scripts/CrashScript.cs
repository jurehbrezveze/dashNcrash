using UnityEngine;

public class CrashScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float initialForce = 10f;     // Initial downward impulse when right-clicking
    public float maxFallSpeed = 20f;     // Cap on how fast the player can fall

    private bool rotationLocked = false;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb == null)
        {
            Debug.LogError("CrashScript: No Rigidbody2D assigned or found.");
        }
    }

    void Update()
    {
        if (rb == null) return;

        // Trigger fall
        if (Input.GetMouseButtonDown(1))
        {
            // Reset & lock rotation
            rb.rotation = 0f;
            rb.freezeRotation = true;
            rotationLocked = true;

            // Add initial downward impulse
            rb.velocity = new Vector2(rb.velocity.x, 0); // Cancel upward movement
            rb.AddForce(Vector2.down * initialForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (rotationLocked)
        {
            rb.freezeRotation = false;
            rotationLocked = false;
        }
    }
}

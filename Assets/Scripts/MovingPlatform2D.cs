using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform2D : MonoBehaviour
{
    [Header("Waypoints (Set 2 empty GameObjects)")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float waitTime = 1f;

    [Header("Collision Settings")]
    public string playerTag = "Player";

    private Transform target;
    private bool waiting = false;
    private bool activated = false;
    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private GameObject playerOnPlatform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        target = pointB;
        lastPosition = rb.position;
    }

    void FixedUpdate()
    {
        if (!activated || waiting || target == null) return;

        Vector2 newPos = Vector2.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);
        Vector2 deltaMove = newPos - rb.position;
        rb.MovePosition(newPos);

        // Move the player manually if they're on top
        if (playerOnPlatform != null)
        {
            playerOnPlatform.transform.position += (Vector3)deltaMove;
            playerOnPlatform.transform.position += Vector3.up * 0.001f; // tiny lift to avoid clipping
        }

        if (Vector2.Distance(rb.position, target.position) < 0.05f)
        {
            StartCoroutine(SwitchTargetAfterDelay());
        }
    }

    System.Collections.IEnumerator SwitchTargetAfterDelay()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        target = (target == pointA) ? pointB : pointA;
        waiting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            // Activate platform no matter where the player hits
            activated = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag(playerTag)) return;

        Rigidbody2D playerRb = collision.collider.attachedRigidbody;
        if (playerRb == null) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Only carry the player if they’re standing on top
            if (contact.normal.y > 0.5f && playerRb.velocity.y <= 0.1f)
            {
                playerOnPlatform = collision.collider.gameObject;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag) && collision.collider.gameObject == playerOnPlatform)
        {
            playerOnPlatform = null;
        }
    }
}

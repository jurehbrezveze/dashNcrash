using UnityEngine;

public class DirectionalBouncePad : MonoBehaviour
{
    public float bounceForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D rb = collision.rigidbody;
        if (rb == null) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal.normalized;
            Vector2 localNormal = transform.InverseTransformDirection(normal);

            Vector2 bounceDirection = Vector2.zero;

            if (Mathf.Abs(localNormal.y) > Mathf.Abs(localNormal.x))
            {
                bounceDirection = localNormal.y < 0 ? transform.up : -transform.up;
            }
            else
            {
                bounceDirection = localNormal.x < 0 ? transform.right : -transform.right;
            }

            rb.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);
            break;
        }
    }
}

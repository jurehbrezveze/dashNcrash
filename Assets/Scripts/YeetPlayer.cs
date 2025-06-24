using UnityEngine;

public class YeetPlayer : MonoBehaviour
{
    public float yeetForce = 15f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Stop current velocity
                rb.velocity = Vector2.zero;

                // Grab pad's Z rotation and rotate it +90° (counterclockwise)
                float zRotation = transform.eulerAngles.z + 90f;

                // Convert to direction
                float radians = zRotation * Mathf.Deg2Rad;
                Vector2 yeetDir = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

                // Apply force
                rb.AddForce(yeetDir * yeetForce, ForceMode2D.Impulse);
            }
        }
    }
}

using UnityEngine;

public class CursedPlatform : MonoBehaviour
{
    [Tooltip("Tag of the player object.")]
    public string playerTag = "Player";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            collision.transform.SetParent(null);
        }
    }
}

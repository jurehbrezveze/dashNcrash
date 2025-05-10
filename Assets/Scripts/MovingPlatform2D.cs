using UnityEngine;

public class MovingPlatform2D : MonoBehaviour
{
    [Header("Waypoints (Set 2 empty GameObjects)")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float waitTime = 1f;

    [Header("Collision Settings")]
    public string playerTag = "Player"; // Ensure your player GameObject is tagged with this

    private Transform target;
    private bool waiting = false;
    private bool activated = false;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        if (!activated || waiting || target == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
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
            activated = true;
        }
    }
}

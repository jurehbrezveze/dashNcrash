using UnityEngine;

public class MovingPlatform2D : MonoBehaviour
{
    [Header("Waypoints (set 2 empty GameObjects)")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement")]
    public float speed = 2f;
    public float waitTime = 1f;

    private Transform target;
    private bool waiting = false;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        if (!waiting && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.05f)
            {
                StartCoroutine(SwitchTargetAfterDelay());
            }
        }
    }

    System.Collections.IEnumerator SwitchTargetAfterDelay()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        target = (target == pointA) ? pointB : pointA;
        waiting = false;
    }
}

using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform player;
    public float baseSmoothSpeed = 5f;
    public float maxSmoothSpeed = 20f; // max speed when player is far
    public float threshold = 6f; // how far from center before speeding up

    void LateUpdate()
    {
        if (player == null) return;

        float targetY = Mathf.Max(player.position.y, 0);
        float distance = Mathf.Abs(targetY - transform.position.y);

        float smoothSpeed = baseSmoothSpeed;

        if (distance > threshold)
        {
            smoothSpeed = Mathf.Lerp(baseSmoothSpeed, maxSmoothSpeed, (distance - threshold) / threshold);
        }

        Vector3 targetPosition = new Vector3(0, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}

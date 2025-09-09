using UnityEngine;

public class Turret2D : MonoBehaviour
{
    [Header("References")]
    public Transform pivot;        // Rotating part (turret head)
    public Transform muzzle;       // Where bullets spawn
    public GameObject bulletPrefab;

    [Header("Settings")]
    public float rotationSpeed = 180f; // Degrees per second
    public float detectionRange = 10f;
    public float fireRate = 1f;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 3f;

    [Header("Rotation")]
    [Tooltip("Offset in degrees to align sprite forward direction.")]
    public float angleOffset = 0f;
    public Transform minAngleTransform; // Empty to mark left bound
    public Transform maxAngleTransform; // Empty to mark right bound

    [Header("Audio & VFX")]
    public AudioSource audioSource;
    public AudioClip detectSound;
    public AudioClip shootSound;
    public ParticleSystem muzzleFlash;

    private Transform player;
    private float fireCooldown;
    private bool playerDetected = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || pivot == null) return;

        Vector2 direction = player.position - pivot.position;
        float distance = direction.magnitude;

        if (distance <= detectionRange)
        {
            // Base angle to player
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply offset
            targetAngle += angleOffset;

            // Clamp to limits if empties are set
            if (minAngleTransform && maxAngleTransform)
            {
                float minAngle = GetRelativeAngle(minAngleTransform.position);
                float maxAngle = GetRelativeAngle(maxAngleTransform.position);

                targetAngle = ClampAngleInSector(targetAngle, minAngle, maxAngle);
            }

            // Smooth rotate toward target (stay inside allowed sector)
            float currentAngle = pivot.eulerAngles.z;
            if (currentAngle > 180f) currentAngle -= 360f; // normalize to -180..180

            // Clamp target into sector
            float clampedTarget = targetAngle;
            if (minAngleTransform && maxAngleTransform)
            {
                float minAngle = GetRelativeAngle(minAngleTransform.position);
                float maxAngle = GetRelativeAngle(maxAngleTransform.position);
                clampedTarget = ClampAngleInSector(targetAngle, minAngle, maxAngle);
            }

            // Rotate toward clampedTarget without cutting through forbidden zone
            float step = rotationSpeed * Time.deltaTime;
            float delta = Mathf.DeltaAngle(currentAngle, clampedTarget);

            if (Mathf.Abs(delta) <= step)
            {
                currentAngle = clampedTarget; // snap if close enough
            }
            else
            {
                currentAngle += Mathf.Sign(delta) * step;
            }

            pivot.rotation = Quaternion.Euler(0, 0, currentAngle);

            // Detection sound (once)
            if (!playerDetected)
            {
                playerDetected = true;
                if (audioSource && detectSound) audioSource.PlayOneShot(detectSound);
            }

            // Shooting
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Fire();
                fireCooldown = 1f / fireRate;
            }
        }
        else
        {
            playerDetected = false;
        }
    }

    void Fire()
    {
        if (!bulletPrefab || !muzzle) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = muzzle.right * bulletSpeed;
        }

        Destroy(bullet, bulletLifetime);

        if (audioSource && shootSound) audioSource.PlayOneShot(shootSound);
        if (muzzleFlash) muzzleFlash.Play();
    }

    float GetRelativeAngle(Vector3 worldPos)
    {
        Vector2 dir = worldPos - pivot.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;
    }

    float ClampAngleInSector(float angle, float min, float max)
    {
        // Normalize to [0..360)
        angle = (angle + 360f) % 360f;
        min = (min + 360f) % 360f;
        max = (max + 360f) % 360f;

        // If sector crosses 0° (e.g. min=300, max=60), adjust logic
        bool crossesZero = min > max;

        if (!crossesZero)
        {
            // Normal case
            return Mathf.Clamp(angle, min, max);
        }
        else
        {
            // Wrap-around case: valid sector is [min..360) U [0..max]
            if (angle >= min || angle <= max)
            {
                return angle; // inside sector, no clamp needed
            }
            else
            {
                // Outside sector → clamp to nearest bound
                float distToMin = Mathf.DeltaAngle(angle, min);
                float distToMax = Mathf.DeltaAngle(angle, max);
                return Mathf.Abs(distToMin) < Mathf.Abs(distToMax) ? min : max;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pivot == null || minAngleTransform == null || maxAngleTransform == null) return;

        Gizmos.color = Color.yellow;

        Vector3 minDir = (minAngleTransform.position - pivot.position).normalized * detectionRange;
        Vector3 maxDir = (maxAngleTransform.position - pivot.position).normalized * detectionRange;

        Gizmos.DrawLine(pivot.position, pivot.position + minDir);
        Gizmos.DrawLine(pivot.position, pivot.position + maxDir);
    }
}

using UnityEngine;

public class Turret2D : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;               // Assign player in Inspector
    public Transform turretTube;           // The rotating cannon part
    public float detectionRange = 15f;     // Turret sight range
    public float rotationSpeed = 5f;       // How fast the tube rotates
    public float minAngle = -60f;          // Minimum rotation limit (relative to right)
    public float maxAngle = 60f;           // Maximum rotation limit

    [Header("Firing")]
    public GameObject projectilePrefab;    // Projectile prefab
    public Transform firePoint;            // Where bullets spawn
    public float projectileSpeed = 20f;
    public float fireDelay = 1.5f;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioSource fireSound;

    private float fireCooldown;

    void Update()
    {
        if (player == null) return;

        // Check distance first
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange) return;

        // Calculate angle towards player
        Vector2 direction = (player.position - turretTube.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Clamp turret rotation
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        // Smooth rotate tube
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        turretTube.rotation = Quaternion.Lerp(turretTube.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Check alignment
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(turretTube.eulerAngles.z, targetAngle));

        // Raycast forward from firePoint
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, detectionRange);

        // Fire only if aligned AND ray hits player
        if (angleDiff < 3f && hit.collider != null && hit.collider.transform == player)
        {
            if (fireCooldown <= 0f)
            {
                Fire();
                fireCooldown = fireDelay;
            }
        }

        // Countdown cooldown
        fireCooldown -= Time.deltaTime;
    }

    void Fire()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = firePoint.right * projectileSpeed;
            }
        }

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (fireSound != null)
            fireSound.Play();
    }
}

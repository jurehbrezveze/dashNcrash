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
    public float minAngle = -90f;
    public float maxAngle = 90f;

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
        if (player == null) return;

        Vector2 direction = player.position - pivot.position;
        float distance = direction.magnitude;

        if (distance <= detectionRange)
        {
            // Base angle to player
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply offset
            targetAngle += angleOffset;

            // Clamp to limits
            targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

            // Smooth rotate toward target
            float currentAngle = pivot.eulerAngles.z;
            if (currentAngle > 180f) currentAngle -= 360f; // normalize -180..180
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

            pivot.rotation = Quaternion.Euler(0, 0, newAngle);

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
}

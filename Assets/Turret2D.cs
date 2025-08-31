using UnityEngine;

public class SimpleTurret2D : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;         // Player target
    public Transform pivot;          // Rotating barrel part
    public Transform muzzle;         // Muzzle spawn point
    public GameObject projectilePrefab;

    [Header("Turret Settings")]
    public float rotationSpeed = 5f;     // How fast the turret turns
    public float rotationOffset = 0f;    // Adjust if sprite isn’t facing right
    public float detectionRange = 15f;   // How far turret can detect

    [Header("Firing")]
    public float projectileSpeed = 20f;
    public float fireDelay = 1.5f;
    public float projectileLifetime = 5f; // How long before bullets despawn

    [Header("Sounds")]
    public AudioSource detectionSound;   // Plays when player first seen
    public AudioSource shootingSound;    // Plays when firing

    private float fireCooldown;
    private bool playerInRange = false;

    void Update()
    {
        if (player == null) return;

        // Rotate pivot towards player
        Vector2 direction = player.position - pivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Check range
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // Play detection sound once when player enters range
            if (!playerInRange)
            {
                if (detectionSound != null) detectionSound.Play();
                playerInRange = true;
            }

            HandleFiring();
        }
        else
        {
            playerInRange = false;
        }
    }

    void HandleFiring()
    {
        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = fireDelay;
        }
        fireCooldown -= Time.deltaTime;
    }

    void Fire()
    {
        if (projectilePrefab != null && muzzle != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = muzzle.right * projectileSpeed;
            }
            // Destroy bullet after lifetime
            Destroy(projectile, projectileLifetime);
        }

        if (shootingSound != null) shootingSound.Play();
    }
}

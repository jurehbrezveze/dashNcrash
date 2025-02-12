using UnityEngine;

public class MouseParticleTrigger : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;  // Assign the particle system in the Inspector
    private Transform particleParent;

    void Start()
    {
        particleParent = transform;  // Make the empty object the parent
    }

    void Update()
    {
        RotateTowardsMouse();

        if (Input.GetMouseButtonDown(0))  // Left mouse click
        {
            TriggerParticleSystem();
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = particleParent.position.z;  // Keep the same Z position as the parent
        Vector3 direction = mousePosition - particleParent.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        particleParent.rotation = Quaternion.Euler(0, 0, angle);
    }

    void TriggerParticleSystem()
    {
        if (particleSystemPrefab != null)
        {
            ParticleSystem particles = Instantiate(particleSystemPrefab, particleParent.position, particleParent.rotation);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);  // Automatically destroy the particles after playing
        }
    }
}

using UnityEngine;

public class PrefabSpawnMainMenuDecor : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject prefabToSpawn;
    public Transform[] spawnPoints;

    [Header("Object Settings")]
    public float moveSpeed = 2f;
    public float lifeTime = 3f;
    public float spawnInterval = 1f;

    [Header("Rotation While Moving")]
    public bool spinWhileMoving = true;
    public float spinSpeed = 45f; // degrees per second

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnAtAllPoints();
        }
    }

    void SpawnAtAllPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Random rotation on Z axis between 0 and 360 degrees
            float rotationZ = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, rotationZ);

            GameObject obj = Instantiate(prefabToSpawn, spawnPoint.position, rotation);

            // Add movement & optional spinning
            AutoMoveUp mover = obj.AddComponent<AutoMoveUp>();
            mover.Initialize(moveSpeed, lifeTime, spinWhileMoving, spinSpeed);
        }
    }
}

public class AutoMoveUp : MonoBehaviour
{
    float speed;
    float lifetime;
    float timer;
    bool spin;
    float spinSpeed;

    public void Initialize(float moveSpeed, float destroyTime, bool spinEnabled, float spinSpeedVal)
    {
        speed = moveSpeed;
        lifetime = destroyTime;
        spin = spinEnabled;
        spinSpeed = spinSpeedVal;
    }

    void Update()
    {
        // Always move straight up in world space
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Optional spinning effect
        if (spin)
        {
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        }

        // Lifetime countdown
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}

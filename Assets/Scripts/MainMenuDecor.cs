using UnityEngine;

public class PrefabSpawnMainMenuDecor : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject prefabToSpawn;
    public Transform[] spawnPoints;

    [Header("Rotation")]
    public float[] possibleRotations = { 0f, 90f, 180f, 270f };

    [Header("Object Settings")]
    public float moveSpeed = 2f;
    public float lifeTime = 3f;
    public float spawnInterval = 1f;

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
            float rotationZ = possibleRotations[Random.Range(0, possibleRotations.Length)];
            Quaternion rotation = Quaternion.Euler(0f, 0f, rotationZ);

            GameObject obj = Instantiate(prefabToSpawn, spawnPoint.position, rotation);
            obj.AddComponent<AutoMoveUp>().Initialize(moveSpeed, lifeTime);
        }
    }
}

public class AutoMoveUp : MonoBehaviour
{
    float speed;
    float lifetime;
    float timer;

    public void Initialize(float moveSpeed, float destroyTime)
    {
        speed = moveSpeed;
        lifetime = destroyTime;
    }

    void Update()
    {
        // Always move straight up in world space
        transform.position += Vector3.up * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}

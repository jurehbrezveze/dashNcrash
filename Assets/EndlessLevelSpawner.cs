using UnityEngine;
using System.Collections.Generic;

public class EndlessLevelSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> chunkPrefabs; // possible stage pieces

    [Header("Settings")]
    public int startChunks = 3; // how many chunks to spawn initially
    public float checkDistance = 10f; // distance from exit before spawning new
    public float yOffset = 0f; // extra spacing added on top of ExitPoint
    public int maxChunks = 50; // cap on total chunks (-1 = infinite)

    private Transform player;
    private List<GameObject> spawnedChunks = new List<GameObject>();
    private Transform lastExitPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Spawn starting chunks stacked upward
        for (int i = 0; i < startChunks; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (player == null) return;

        // Only spawn if under max cap
        if (maxChunks < 0 || spawnedChunks.Count < maxChunks)
        {
            if (lastExitPoint != null &&
                player.position.y + checkDistance >= lastExitPoint.position.y)
            {
                SpawnNextChunk();
            }
        }
    }

    void SpawnNextChunk()
    {
        if (maxChunks >= 0 && spawnedChunks.Count >= maxChunks)
            return; // don’t spawn more if cap reached

        // Pick a random chunk prefab
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];

        Vector3 spawnPos;

        if (lastExitPoint != null)
        {
            // Align prefab’s bottom with lastExitPoint + Y offset
            ChunkExit prefabExit = prefab.GetComponentInChildren<ChunkExit>();
            if (prefabExit != null)
            {
                float exitLocalY = prefabExit.transform.localPosition.y;
                spawnPos = lastExitPoint.position - new Vector3(0, exitLocalY, 0);

                // Apply custom Y offset
                spawnPos.y += yOffset;
            }
            else
            {
                Debug.LogWarning("Chunk prefab missing a ChunkExit!");
                spawnPos = lastExitPoint.position + new Vector3(0, yOffset, 0);
            }
        }
        else
        {
            // First chunk at origin
            spawnPos = Vector3.zero;
        }

        // Instantiate the chunk
        GameObject newChunk = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedChunks.Add(newChunk);

        // Update lastExitPoint to this new chunk’s exit
        ChunkExit exit = newChunk.GetComponentInChildren<ChunkExit>();
        if (exit != null)
        {
            lastExitPoint = exit.transform;
        }
    }
}

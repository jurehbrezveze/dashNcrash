using UnityEngine;
using System.Collections;

public class CubeRain : MonoBehaviour
{
    public GameObject cubePrefab;
    public int cubesPerSecond = 20;
    public float spawnWidth = 20f;
    public float spawnHeight = 10f;
    public float cubeLifetime = 3f;

    void Start()
    {
        // Start raining cubes immediately
        StartCoroutine(RainCubes());
    }

    IEnumerator RainCubes()
    {
        while (true)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(-spawnWidth / 2f, spawnWidth / 2f),
                spawnHeight,
                0f
            );

            GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
            Destroy(cube, cubeLifetime);

            yield return new WaitForSeconds(1f / cubesPerSecond);
        }
    }
}

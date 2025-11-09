using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject enemyPrefab; // Assign enemy prefab in Inspector
    [SerializeField] private Transform player;       // Reference to player
    [SerializeField] private float spawnRadius = 10f; // Distance from player where enemies appear
    public float initialSpawnDelay = 2f; // Initial delay between spawns
    public float spawnAcceleration = 0.05f; // How fast spawn rate increases
    public float minSpawnDelay = 0.5f; // Smallest delay possible

    private float currentSpawnDelay;

    // ✅ Add this public property so other scripts (like BGM controller) can read it
    public float CurrentSpawnDelay => currentSpawnDelay;

    void Start()
    {
        currentSpawnDelay = initialSpawnDelay;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(currentSpawnDelay);

            // Gradually increase spawn rate
            if (currentSpawnDelay > minSpawnDelay)
                currentSpawnDelay -= spawnAcceleration;
        }
    }

    void SpawnEnemy()
    {
        if (!player) return;

        // Random point around player in a circle
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}

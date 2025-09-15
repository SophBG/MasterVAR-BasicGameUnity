using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius;       // Radius around player to spawn enemies
    public float spawnInterval;     // Time between spawn attempts
    public int maxEnemies;          // Maximum concurrent enemies

    [Header("Player Reference")]
    public Transform player;

    private int currentEnemyCount = 0;      // Track currently active enemies

    void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnEnemiesRoutine());
    }

    // Coroutine that handles enemy spawning at regular intervals
    IEnumerator SpawnEnemiesRoutine()
    {
        // Wait a moment before starting to spawn
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn if we haven't reached the enemy limit
            if (currentEnemyCount < maxEnemies)
            {
                // Find a spawn position with ground below
                Vector3 spawnPosition = FindSpawnPosition();
                SpawnEnemyAtPosition(spawnPosition);
            }
        }
    }

    // Finds a valid spawn position with ground below within spawn radius
    Vector3 FindSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 randomPoint = player.position + new Vector3(randomDirection.x, 0, randomDirection.y) * spawnRadius;
        return randomPoint;
    }

    // Spawns an enemy at the specified position
    void SpawnEnemyAtPosition(Vector3 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        
        // Make enemy face towards player
        enemy.transform.LookAt(new Vector3(player.position.x, position.y, player.position.z));
        
        // Pass player reference to enemy movement script
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.Initialize(player);
        }
        
        // Setup enemy reference to this spawner for cleanup
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.Initialize(this);
        }
        
        // Increment enemy count
            currentEnemyCount++;
    }

    // Called by enemies when they are destroyed to update count
    public void NotifyEnemyDestroyed()
    {
        currentEnemyCount--;
        currentEnemyCount = Mathf.Max(0, currentEnemyCount); // Ensure never goes below 0
        Debug.Log($"Enemy destroyed. Remaining: {currentEnemyCount}");
    }
}
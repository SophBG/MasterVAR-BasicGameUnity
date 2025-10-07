using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;          // Prefab of the enemy to spawn
    public float spawnInterval;             // Time between spawn attempts
    public int maxEnemies;                  // Maximum number of concurrent enemies

    [Header("Spawn Radius Settings")]
    public float minSpawnRadius = 5f;       // Minimum distance from player to spawn
    public float maxSpawnRadius = 20f;      // Maximum distance from player to spawn

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;           // Layer mask for ground detection
    public LayerMask obstacleLayer;         // Layer mask for obstacle detection
    public float maxGroundCheckDistance = 50f; // Maximum distance to check for ground
    public int maxSpawnAttempts = 10;       // Maximum attempts to find valid spawn position

    [Header("Player Reference")]
    public Transform player;                // Reference to player transform

    private int currentEnemyCount = 0;      // Current number of active enemies

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        // Initial delay before starting spawns
        yield return new WaitForSeconds(1f);

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Only spawn if under enemy limit
            if (currentEnemyCount < maxEnemies)
            {
                Vector3? spawnPosition = FindValidSpawnPosition();
                if (spawnPosition.HasValue)
                {
                    SpawnEnemyAtPosition(spawnPosition.Value);
                }
            }
        }
    }

    private Vector3? FindValidSpawnPosition()
    {
        // Try multiple positions to find valid spawn
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            // Generate random point between min and max radius
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 randomPoint = player.position + new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;

            if (IsValidSpawnPosition(randomPoint))
            {
                return randomPoint;
            }
            /*
            else
            {
                Debug.Log("Position " + randomPoint.ToString() + " not valid\n");
            }
            */
        }

        return null; // No valid position found
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Raycast downward to find ground
        if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out RaycastHit hit, maxGroundCheckDistance, groundLayer))
        {
            Vector3 groundPosition = hit.point;
            
            // Check if position is within allowed radius range
            float distanceToPlayer = Vector3.Distance(groundPosition, player.position);
            if (distanceToPlayer < minSpawnRadius || distanceToPlayer > maxSpawnRadius)
                return false;

            // Check for obstacles at spawn location
            if (Physics.CheckSphere(groundPosition + Vector3.up * 0.5f, 0.5f, obstacleLayer))
                return false;

            // Check surface slope (avoid steep terrain)
            if (Vector3.Angle(hit.normal, Vector3.up) > 30f)
                return false;

            return true;
        }

        return false; // No ground found
    }

    private void SpawnEnemyAtPosition(Vector3 position)
    {
        // Final ground alignment before spawning
        if (Physics.Raycast(position + Vector3.up * 2f, Vector3.down, out RaycastHit hit, maxGroundCheckDistance, groundLayer))
        {
            position = hit.point + Vector3.up * 0.1f; // Spawn slightly above ground

            GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
            
            // Make enemy face player
            enemy.transform.LookAt(new Vector3(player.position.x, position.y, player.position.z));
            
            // Initialize enemy components
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.Initialize(player);
            }
            
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Initialize(this);
            }
            
            currentEnemyCount++;
        }
    }

    public void NotifyEnemyDestroyed()
    {
        currentEnemyCount--;
        currentEnemyCount = Mathf.Max(0, currentEnemyCount);
    }
}
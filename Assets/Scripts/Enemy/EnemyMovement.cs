using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private Transform playerTarget;
    
    [Header("Movement Settings")]
    public float movementSpeed;
    public float rotationSpeed;
    
    // Initialize with references from spawner
    public void Initialize(Transform playerReference)
    {
        playerTarget = playerReference;
        enemyAgent = GetComponent<NavMeshAgent>();
        
        // Apply custom speeds to NavMeshAgent
        if (enemyAgent != null)
        {
            enemyAgent.speed = movementSpeed;
            enemyAgent.angularSpeed = rotationSpeed;
        }
    }

    void Update()
    {
        // Update destination every frame to follow moving player
        if (playerTarget != null && enemyAgent != null)
        {
            enemyAgent.SetDestination(playerTarget.position);
        }
    }
}
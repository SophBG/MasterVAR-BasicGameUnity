using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private Transform playerTarget;
    
    // Initialize with references from spawner
    public void Initialize(Transform playerReference)
    {
        playerTarget = playerReference;
        enemyAgent = GetComponent<NavMeshAgent>();
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
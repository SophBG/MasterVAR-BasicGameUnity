using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemySpawner spawner;

    // Sets reference to the spawner that created this enemy
    public void Initialize(EnemySpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    // Notify spawner when enemy is destroyed
    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.NotifyEnemyDestroyed();
        }
    }
}
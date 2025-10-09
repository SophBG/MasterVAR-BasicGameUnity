using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;          // Maximum health value
    public Slider healthBar;            // Enemy health bar
    public int currentHealth;           // Current health value
    
    private EnemySpawner spawner;   // Reference to spawner
    private Transform playerCam;    // Player cam for bar rotation

    private bool isDead = false;    // Death state flag

    public void LateUpdate()
    {
        healthBar.transform.LookAt(playerCam);
    }

    public void Initialize(EnemySpawner spawnerRef, Transform cam)
    {
        spawner = spawnerRef;
        currentHealth = maxHealth;
        healthBar.value = 100;
        playerCam = cam;
    }
    
    public void TakeDamage(int damageAmount)
    {
        // Apply damage and check for death
        if (isDead) return;
        
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);

        healthBar.value = 100 * currentHealth / maxHealth;

        spawner.NotifyEnemyDamaged();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Handle enemy death
        isDead = true;
        
        if (spawner != null)
        {
            spawner.NotifyEnemyDestroyed();
        }
        
        Destroy(gameObject, 0.1f);
    }
}
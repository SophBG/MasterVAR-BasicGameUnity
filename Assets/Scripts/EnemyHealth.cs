using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;          // Maximum health value
    public int currentHealth;           // Current health value
    
    [Header("Events")]
    public UnityEvent OnDamageTaken;                    // Event when damage is taken
    public UnityEvent OnDeath;                          // Event when enemy dies
    public UnityEvent<int> OnDamageTakenWithAmount;     // Event with damage amount
    public UnityEvent<int, int> OnHealthChanged;        // Event with current and max health
    
    private EnemySpawner spawner;   // Reference to spawner
    private bool isDead = false;    // Death state flag
    
    public void Initialize(EnemySpawner spawnerRef)
    {
        spawner = spawnerRef;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public void TakeDamage(int damageAmount)
    {
        // Apply damage and check for death
        if (isDead) return;
        
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke();
        OnDamageTakenWithAmount?.Invoke(damageAmount);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Handle enemy death
        isDead = true;
        OnDeath?.Invoke();
        
        if (spawner != null)
        {
            spawner.NotifyEnemyDestroyed();
        }
        
        Destroy(gameObject, 0.1f);
    }
}
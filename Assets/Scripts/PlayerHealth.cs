using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    
    [Header("Damage Settings")]
    public float damageCooldown = 1f; // Time between damage instances
    
    [Header("Events")]
    public UnityEvent OnDamageTaken;
    public UnityEvent OnDeath;
    public UnityEvent<int, int> OnHealthChanged; // Current health, max health
    
    private float lastDamageTime;
    private bool isDead = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        // Check if we're still in damage cooldown
        // if (Time.time - lastDamageTime < damageCooldown) return;
        // lastDamageTime = Time.time;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // probably will not be used on time
    public void Heal(int healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        
        // Debug.Log("Player has died!");
    }
}
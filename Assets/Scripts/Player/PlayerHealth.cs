using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public Slider healthBar;            // Player health bar
    public int currentHealth;
    
    [Header("Damage Settings")]
    public float damageCooldown = 1f; // Time between damage instances
    
    [Header("Events")]
    public UnityEvent OnDeath;
    
    private float lastDamageTime;
    private bool isDead = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = 100;
    }
    
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        // Check if we're still in damage cooldown
        // if (Time.time - lastDamageTime < damageCooldown) return;
        // lastDamageTime = Time.time;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);

        healthBar.value = 100 * currentHealth / maxHealth;
        
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
    }
    
    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        
        // Debug.Log("Player has died!");
    }
}
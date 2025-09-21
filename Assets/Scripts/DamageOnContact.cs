using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 10;
    public float damageCooldown = 1f;
    
    [Header("Filter Settings")]
    public bool checkByTag = true;          // check by tag for extra safety
    public string playerTag = "Player";
    
    private float lastDamageTime;
    
    private void OnCollisionEnter(Collision collision)
    {
        CheckForPlayerDamage(collision.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CheckForPlayerDamage(other.gameObject);
    }
    
    private void CheckForPlayerDamage(GameObject otherObject)
    {
        // check if the object has PlayerHealth component
        PlayerHealth playerHealth = otherObject.GetComponent<PlayerHealth>();
        
        if (playerHealth != null && otherObject.CompareTag(playerTag))
        {
            TryDealDamage(playerHealth);
        }
    }
    
    private void TryDealDamage(PlayerHealth targetPlayerHealth)
    {
        if (targetPlayerHealth != null && Time.time - lastDamageTime >= damageCooldown)
        {
            targetPlayerHealth.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
        }
    }
}
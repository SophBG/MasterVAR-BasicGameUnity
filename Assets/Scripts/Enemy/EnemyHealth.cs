using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth;           // Maximum health value
    public Slider healthBar;        // Enemy health bar
    public int currentHealth;       // Current health value

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip idleSound;
    [Range(0f, 1f)] public float idleVolume;
    public float minIdleTime;
    public float maxIdleTime;
    
    private EnemySpawner spawner;   // Reference to spawner
    private Transform playerCam;    // Player cam for bar rotation
    private float nextIdleSoundTime;
    private bool isDead = false;    // Death state flag

    public void LateUpdate()
    {
        healthBar.transform.LookAt(playerCam);

        if (!isDead && Time.time >= nextIdleSoundTime)
        {
            PlayIdleSound();
            ScheduleNextIdleSound();
        }
    }

    public void Initialize(EnemySpawner spawnerRef, Transform cam)
    {
        spawner = spawnerRef;
        currentHealth = maxHealth;
        healthBar.value = 100;
        playerCam = cam;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        ScheduleNextIdleSound();
    }
    
    public void TakeDamage(int damageAmount)
    {
        // Apply damage and check for death
        if (isDead) return;
        
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);

        healthBar.value = 100 * currentHealth / maxHealth;

        spawner.NotifyEnemyDamaged(damageAmount);
        
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

    private void PlayIdleSound()
    {
        if (!isDead && !audioSource.isPlaying && idleSound != null)
        {
            audioSource.PlayOneShot(idleSound, idleVolume);
        }
    }
    
    private void ScheduleNextIdleSound()
    {
        nextIdleSoundTime = Time.time + Random.Range(minIdleTime, maxIdleTime);
    }
}
using UnityEngine;
using System.Collections;

public class DamageOnContact : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount;
    public float damageCooldown;
    
    [Header("Filter Settings")]
    public bool checkByTag = true;          // check by tag for extra safety
    public string playerTag = "Player";

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip damageSound;
    [Range(0f, 1f)] public float damageVolume;

    private float lastDamageTime;
    private bool isInContact = false;
    private PlayerHealth currentPlayerHealth;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (CheckIfPlayer(collision.gameObject))
        {
            isInContact = true;
            StartCoroutine(DamageOverTime());
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (CheckIfPlayer(collision.gameObject))
        {
            isInContact = false;
        }
    }

    private bool CheckIfPlayer(GameObject otherObject)
    {
        // check if the object has PlayerHealth component

        PlayerHealth playerHealth = otherObject.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null && (!checkByTag || otherObject.CompareTag(playerTag)))
        {
            currentPlayerHealth = playerHealth;
            return true;
        }
        return false;
    }
    
    private IEnumerator DamageOverTime()
    {
        while (isInContact)
        {
            TryDealDamage(currentPlayerHealth);
            yield return new WaitForSeconds(damageCooldown);
        }
    }

    private void TryDealDamage(PlayerHealth targetPlayerHealth)
    {
        if (targetPlayerHealth != null && Time.time - lastDamageTime >= damageCooldown)
        {
            targetPlayerHealth.TakeDamage(damageAmount);
            PlayDamageSound();
            lastDamageTime = Time.time;
        }
    }
    
    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound, damageVolume);
        }
    }
}
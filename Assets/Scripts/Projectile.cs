using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage; // Damage value of the bullet
    public float lifeTime = 1; // Time before bullet is destroyed

    private void Update()
    {
        // Countdown lifetime and destroy when expired
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage((int)damage);
            }
        }

        if (!other.CompareTag("Projectile") && !other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
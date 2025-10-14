using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage; // Damage value of the bullet
    public float lifeTime; // Time before bullet is destroyed

    public void Initialize(float dam)
    {
        damage = dam;
    }
    
    private void Update()
    {
        // Countdown lifetime and destroy when expired
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage((int)damage);
            }
        }

        if (!collision.gameObject.CompareTag("Projectile") && !collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
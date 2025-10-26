using UnityEngine;

public class FireballDamage : MonoBehaviour
{
    public float damage = 10f;
    public float destroyDelay = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector2 knockback = Vector2.zero;
                enemy.TakeDamage(damage, knockback);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            // Không phá h?y n?u va ch?m chính Player
        }
        else if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}

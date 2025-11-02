using UnityEngine;

public class PoisonFireball : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float initialDamage = 10f;
    [SerializeField] private int poisonDamagePerSecond = 3;
    [SerializeField] private float poisonDuration = 5f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(initialDamage);
                player.ApplyPoison((int)poisonDamagePerSecond, poisonDuration);
            }

            Destroy(gameObject);
        }
    }
}

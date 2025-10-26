using UnityEngine;

public class FallingSwordController : MonoBehaviour
{
    public float damage = 10f;
    public float fallDuration = 2f;
    private Vector3 targetPosition;
    private float timer = 0f;
    private bool isFalling = false;

    public void Initialize(Vector3 target)
    {
        targetPosition = target;
        isFalling = true;
    }

    private void Update()
    {
        if (!isFalling) return;

        timer += Time.deltaTime;
        float t = timer / fallDuration;

        transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit something: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit Player");

            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                Debug.Log("PlayerController found. Damaging & Destroying...");
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Collision has Player tag but NO PlayerController component!");
            }
        }
    }
}
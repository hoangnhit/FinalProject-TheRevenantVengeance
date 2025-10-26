using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour
{
    private Vector2 moveDirection;
    public float speed = 10f;
    public int damage = 10;
    public float lifeTime = 5f;

    private Transform target;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, lifeTime); 
    }
    void Start()
    {
        
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger) 
        {
            Destroy(gameObject);
        }
    }
}

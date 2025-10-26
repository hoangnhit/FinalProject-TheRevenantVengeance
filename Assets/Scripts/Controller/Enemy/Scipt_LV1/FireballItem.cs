using UnityEngine;

public class FireballItem : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Gây sát th??ng cho Enemy (n?u Enemy có script phù h?p)
            Debug.Log("Enemy trúng ??n!");
            Destroy(gameObject);
        }
    }
}

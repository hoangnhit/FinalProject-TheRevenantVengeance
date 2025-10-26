//using UnityEngine;

//public class PowerUpItem : MonoBehaviour
//{
//    public float lifeTime = 10f; // T? h?y sau vài giây n?u không nh?t

//    private void Start()
//    {
//        Destroy(gameObject, lifeTime);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            collision.GetComponent<PlayerController>()?.SetFireballPrefab();  // Kích ho?t skill
//            Destroy(gameObject); // Bi?n m?t sau khi nh?t
//        }
//    }
//}

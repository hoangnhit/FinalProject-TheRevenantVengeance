//using UnityEngine;

//public class ItemPickupLV1 : MonoBehaviour
//{
//    public GameObject fireballPrefab;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            PlayerController player = collision.GetComponent<PlayerController>();
//            if (player != null)
//            {
//                player.SetFireballPrefab(fireballPrefab);
//            }
//            Destroy(gameObject);
//        }
//    }

//}
// ItemPickupLV1.cs
//using UnityEngine;

//public class ItemPickupLV1 : MonoBehaviour
//{
//    public GameObject fireballPrefab;
//    [SerializeField] private GameObject auraPrefab;  // Kéo prefab hào quang vào ?ây trong Inspector

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            PlayerController player = collision.GetComponent<PlayerController>();
//            if (player != null)
//            {
//                // Gán kh? n?ng b?n qu? c?u
//                player.SetFireballPrefab(fireballPrefab);

//                // Kích ho?t hào quang luôn
//                player.ActivateAura(auraPrefab);
//                PlayerStateLv1.acquiredFireball = true;
//                PlayerStateLv1.acquiredAura = true;
//                PlayerStateLv1.savedFireballPrefab = fireballPrefab;
//                PlayerStateLv1.savedAuraPrefab = auraPrefab;
//            }

//            Destroy(gameObject);  // Xóa item sau khi nh?t
//        }
//    }
//}

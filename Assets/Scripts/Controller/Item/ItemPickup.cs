//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.Scripts.Controller.Item
//{
//    public class ItemPickup : MonoBehaviour
//    {
//        [SerializeField] private AudioClip pickupSound;
//        private AudioSource audioSource;

//        private void Awake()
//        {
//            audioSource = GetComponent<AudioSource>();
//            if (audioSource == null)
//            {
//                audioSource = gameObject.AddComponent<AudioSource>();
//            }

//            audioSource.playOnAwake = false;
//        }

//        private void OnTriggerEnter2D(Collider2D collision)
//        {
//            if (collision.CompareTag("Player"))
//            {
//                //PlayerState.acquiredSwordSpin = true;
//                //Destroy(gameObject);
//                PlayerController player = collision.GetComponent<PlayerController>();
//                if (player != null)
//                {
//                    PlayerState.acquiredSwordSpin = true;
//                    player.ShowCircleEffect(); // Gọi vòng tròn ngay khi nhặt
//                }
//                if (pickupSound != null)
//                {
//                    AudioSource.PlayClipAtPoint(pickupSound, transform.position, 2.0f);
//                }

//                Destroy(gameObject); // Xóa item sau khi nhặt
//            }
//        }
//    }
//}
using UnityEngine;

namespace Assets.Scripts.Controller.Item
{
    public enum ItemType
    {
        SwordSpin,
        FireballAndAura
    }
    public class ItemPickup : MonoBehaviour
    {
        public ItemType itemType;

        // Dùng cho Fireball & Aura
        public GameObject fireballPrefab;
        [SerializeField] private GameObject auraPrefab;

        // Dùng cho SwordSpin
        [SerializeField] private AudioClip pickupSound;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.playOnAwake = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();

                switch (itemType)
                {
                    case ItemType.SwordSpin:
                        if (player != null)
                        {
                            PlayerState.acquiredSwordSpin = true;
                            player.ShowCircleEffect();
                        }
                        if (pickupSound != null)
                        {
                            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 2.0f);
                        }
                        break;

                    case ItemType.FireballAndAura:
                        if (player != null)
                        {
                            player.SetFireballPrefab(fireballPrefab);
                            player.ActivateAura(auraPrefab);

                            PlayerState.acquiredFireball = true;
                            PlayerState.acquiredAura = true;
                            PlayerState.savedFireballPrefab = fireballPrefab;
                            PlayerState.savedAuraPrefab = auraPrefab;
                        }
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}

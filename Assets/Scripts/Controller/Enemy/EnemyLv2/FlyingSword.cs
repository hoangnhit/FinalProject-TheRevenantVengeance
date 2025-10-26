using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemy.EnemyLv2
{
    public class FlyingSword : MonoBehaviour
    {
        public float speed = 8f;
        public float damage = 15f;
        public float lifetime = 5f;

        private Vector3 direction;

        public void Initialize(Vector3 targetPosition)
        {
            direction = (targetPosition - transform.position).normalized;

            // Xoay sprite theo hướng bay
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // trừ 90 độ nếu sprite mặc định hướng lên
        }

        void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }

                Destroy(gameObject);
            }
            else if (!collision.isTrigger)
            {
                Destroy(gameObject); // Va vào tường cũng biến mất
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemy.EnemyLv2
{
    public class SwordSpin : MonoBehaviour
    {
        public float damage = 10f;
        public float knockbackForce = 5f;
        public Transform bossCenter;
        public float rotateSpeed = 200f;
        public float radius = 2f;
        public float lifetime = 5f;
        [HideInInspector] public float angleOffset;

        public string ownerTag = "Enemy";
        void Start()
        {
            if (lifetime > 0)
            {
                Destroy(gameObject, lifetime);
            }
        }

        void Update()
        {
            if (bossCenter == null) return;

            float angle = Time.time * rotateSpeed + angleOffset;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            transform.position = bossCenter.position + offset;

            Vector3 direction = transform.position - bossCenter.position;
            float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angleDeg - 45f);
        }

        public void InitPosition()
        {
            if (bossCenter == null) return;

            float angle = Time.timeSinceLevelLoad * rotateSpeed + angleOffset;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            transform.position = bossCenter.position + offset;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Nếu là kiếm của enemy, chỉ đánh player
            if (ownerTag == "Enemy" && collision.CompareTag("Player"))
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    
                }
            }

            // Nếu là kiếm của player, chỉ đánh enemy
            else if (ownerTag == "Player" && collision.CompareTag("Enemy"))
            {
                EnemyController enemy = collision.GetComponentInParent<EnemyController>();
                if (enemy != null && !enemy.IsDead())
                {
                    enemy.TakeDamage(damage, Vector2.zero);
                    
                }
            }
        }
    }
}

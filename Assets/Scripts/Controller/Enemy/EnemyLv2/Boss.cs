using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controller.Enemy.EnemyLv2
{
    public class Boss : EnemyController
    {
        [SerializeField] private GameObject bulletPrefabs;
        [SerializeField] private Transform firePoint;
        //[SerializeField] private float normalAttackSpeed = 20f;
        [SerializeField] private float specialAttackSpeed = 5f;
        [SerializeField] private float hpValue = 20f;
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private Transform swordCenter;

        [SerializeField] private GameObject flyingSwordPrefab;
        [SerializeField] private float skillCooldown = 10f;
        private float nextSkill;

        [SerializeField] private GameObject itemPrefab;

        private Animator animator;
        private bool isAttacking = false;
        private readonly float attackCooldown = 1f;
        private float lastAttackTime = 0f;
        //protected bool isDead = false;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip takeHitSound;
        [SerializeField] private AudioClip deathSound;

        [SerializeField] private GateTriggerBoss gateTrigger;
        [SerializeField] private GameManager gameManager;
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (IsDead() || player == null || player.currentHp <= 0) return;
            base.Update();

            NormalAttack();
            UseSkill();
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        player.TakeDamage(enterDamage);
        //    }
        //}

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        player.TakeDamage(stayDamage);
        //    }
        //}

        private void NormalAttack()
        {        
            if (IsDead() || isAttacking || player == null) return;

            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance > 4f) return;

            if (Time.time - lastAttackTime < attackCooldown) return;

            isAttacking = true;
            lastAttackTime = Time.time;

            animator?.SetTrigger("Attack");
            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
            Invoke(nameof(ResetAttack), attackCooldown);
        }
        private void ResetAttack()
        {
            isAttacking = false;
        }
        private void DealDamageFromAttack1()
        {
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
                player.TakeDamage(enterDamage);
        }

        private void DealDamageFromAttack2()
        {
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
                player.TakeDamage(enterDamage * 1.5f);
        }
        private void DealDamageFromAttack3()
        {
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
                player.TakeDamage(enterDamage * 1.8f);
        }
        public override void TakeDamage(float damage, Vector2 knockbackDirection)
        {
            base.TakeDamage(damage, knockbackDirection);
            if (currentHp > 0)
            {
                animator?.SetTrigger("TakeHit");
                if (audioSource != null && takeHitSound != null)
                {
                    audioSource.PlayOneShot(takeHitSound);
                }
            }
        }

        private void SpecialAttack()
        {
            const int bulletCount = 12;
            float angleStep = 360 / bulletCount;
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
                GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
                EnemyBulletController enemyBullet = bullet.AddComponent<EnemyBulletController>();
                enemyBullet.SetMovementDirection(bulletDirection * specialAttackSpeed);
            }

        }

        private void Heal(float hpAmount)
        {
            currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
            UpdateHealthBar();

        }

        private void SpawnSwordSpin()
        {
            int swordCount = 5;
            for (int i = 0; i < swordCount; i++)
            {
                GameObject sword = Instantiate(swordPrefab, swordCenter.position, Quaternion.identity);
                sword.transform.SetParent(swordCenter); // gắn vào boss để quay theo
                SwordSpin spin = sword.GetComponent<SwordSpin>();
                spin.ownerTag = "Enemy";
                spin.bossCenter = swordCenter;
                spin.angleOffset = i * 360f / swordCount; // chia đều góc
                spin.InitPosition();              
            }
        }

        private void Teleport()
        {
            if (player != null)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float distanceFromPlayer = 3f;

                Vector3 teleportPosition = player.transform.position + (Vector3)(randomDirection * distanceFromPlayer);

                transform.position = teleportPosition;
            }
        }

        private void ShootSwordAtPlayer()
        {
            if (player == null || IsDead()) return;

            GameObject sword = Instantiate(flyingSwordPrefab, transform.position, Quaternion.identity);
            FlyingSword swordScript = sword.GetComponent<FlyingSword>();
            if (swordScript != null)
            {
                swordScript.Initialize(player.transform.position);
            }
        }

        private void ChooseRandomSkill()
        {
            int randomSkill = Random.Range(0, 4);
            switch (randomSkill)
            {
                case 0:
                    SpecialAttack();
                    break;
                case 1:
                    Heal(hpValue);
                    break;
                case 2:
                    SpawnSwordSpin();
                    break;
                case 3:
                    Teleport();
                    break;
                case 4:
                    ShootSwordAtPlayer();
                    break;
            }
        }

        private void UseSkill()
        {
            if (Time.time >= nextSkill)
            {
                nextSkill = Time.time + skillCooldown;
                ChooseRandomSkill();
            }

        }

        protected override void Die()
        {
            //isDead = true;
            animator?.SetTrigger("Die");
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
            if (itemPrefab != null)
            {
                GameObject itemBoss = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                Destroy(itemBoss, 10f);
            }
                       
            if (gateTrigger != null)
            {
                gateTrigger.OpenGate();
            }
            gameManager.OnBossDead();
            //Destroy(gameObject, 2f);
            base.Die();
        }
    }
}

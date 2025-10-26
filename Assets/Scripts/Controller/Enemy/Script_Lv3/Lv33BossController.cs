using System.Collections;
using UnityEngine;

//Ki nang: ban thuong: ban 3 cau` lua vao player
//         ban dac biet: ban 1 vong xoay 100 vien dan hinh xoan oc
//         mua kiem : ban 3 thanh kiem roi roi xuong player
//         hoi mau: hoi 20 mau
//         sinh ra 5 quai nho xung quanh
//         dich chuyen toi player
//         tuc' gian: khi mau duoi 50% thi tuc gian, tang toc do, tang sat thuong, tang dam va tang toc do chay
//         va hoi lai day mau, su dung skill nhieu hon

public class Lv33BossController : EnemyController
{
    [SerializeField] private GameObject[] swordPrefabs;
    [SerializeField] private GameObject fireballPrefabs;
    public GameObject sprialBulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float normalAttackSpeed = 20f;
    [SerializeField] private float specialAttackSpeed = 5f;
    [SerializeField] private float hpValue = 20f;
    [SerializeField] private GameObject miniEnemy;

    [SerializeField] private float shotDelay = 0.2f;
    private float nextShot;
    [SerializeField] private float skillCooldown = 10f;
    private float nextSkill;

    [SerializeField] private GameObject itemPrefab;
    private float summonCooldown = 2.5f;
    private float nextSummonTime = 0f;
    private bool enraged = false;
    private bool isInvincible = false;
    private float spiralAngle = 0f;

    [SerializeField] private float meleeAttackCooldown = 3f;
    private float nextMeleeAttackTime = 0f;

    private Animator animator;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip fireBallSound;
    [SerializeField] private AudioClip waveSound;
    [SerializeField] private AudioClip teleportSound;


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time >= nextSummonTime)
        {
            nextSummonTime = Time.time + summonCooldown;
            NormalAttack();
            if (enraged)
            {
                SpecialAttack();
            }
        }

        if (!enraged && currentHp <= maxHp * 0.5f)
        {
            EnterEnragedState();
        }
        UseSkill();
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < 2.5f && Time.time >= nextMeleeAttackTime)
            {
                nextMeleeAttackTime = Time.time + meleeAttackCooldown;

                float roll = Random.Range(0f, 1f);
                if (roll <= 0.75f)
                {
                    animator.SetTrigger("Attack");
                    if (audioSource != null && attackSound != null)
                    {
                        audioSource.PlayOneShot(attackSound);
                    }
                    player.TakeDamage(20f);
                }
                else
                {
                    isInvincible = false;
                    animator.SetTrigger("Combo");
                    //StartCoroutine(EndInvincibilityAfterAnimation("Combo"));
                    if (audioSource != null && attackSound != null)
                    {
                        audioSource.PlayOneShot(attackSound);
                    }
                    player.TakeDamage(50f);
                }
            }
        }
    }
    private IEnumerator EndInvincibilityAfterAnimation(string animationName)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        while (!state.IsName(animationName))
        {
            yield return null;
            state = animator.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(state.length);

        isInvincible = false;
    }

    public override void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (isInvincible) return; 

        base.TakeDamage(damage, knockbackDirection);
    }


    private void EnterEnragedState()
    {
        enraged = true;

        Debug.Log("Boss has entered ENRAGED state!");


        currentHp = maxHp;
        UpdateHealthBar();

        enterDamage *= 1.5f;
        stayDamage *= 1.5f;
        enemySpeed *= 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(stayDamage);
        }
    }

    //ban 3 qua cau lua vao player
    private void NormalAttack()
    {
        if (player != null && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;

            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();

            float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            float[] angleOffsets = { 0f, -15f, 15f };

            foreach (float offset in angleOffsets)
            {
                float angle = baseAngle + offset;
                float radian = angle * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);

                GameObject bullet = Instantiate(fireballPrefabs, firePoint.position, Quaternion.Euler(0, 0, angle - 270));

                EnemyBulletController enemyBullet = bullet.AddComponent<EnemyBulletController>();
                if (audioSource != null && fireBallSound != null)
                {
                    audioSource.PlayOneShot(fireBallSound);
                }
                enemyBullet.SetMovementDirection(dir * normalAttackSpeed);
            }
        }
    }

    private void SpecialAttack()
    {
        if (sprialBulletPrefabs == null)
        {
            Debug.LogWarning("Spiral bullet prefab is not assigned!", this);
            return;
        }
        if (audioSource != null && waveSound != null)
        {
            audioSource.PlayOneShot(waveSound);
        }
        StartCoroutine(SpiralAttackCoroutine());
    }

    private IEnumerator SpiralAttackCoroutine()
    {
        int bulletsToFire = 100; 
        float angleStep = 10f; 
        float delayBetweenBullets = 0.05f; 

        for (int i = 0; i < bulletsToFire; i++)
        {
            float angle = spiralAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            GameObject bullet = Instantiate(sprialBulletPrefabs, transform.position, Quaternion.identity);
            EnemyBulletController enemyBullet = bullet.AddComponent<EnemyBulletController>();
            enemyBullet.SetMovementDirection(direction * specialAttackSpeed);

            yield return new WaitForSeconds(delayBetweenBullets);
        }

        spiralAngle += 15f; 
    }

    private void Heal(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHealthBar();

    }

    private void SpawnMiniEnemy()
    {
        float radius = 2f;
        for (int i = 0; i < 5; i++)
        {
            float angle = i * Mathf.PI * 2 / 5;
            Vector3 spawnPos = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Instantiate(miniEnemy, spawnPos, Quaternion.identity);

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

    private void SummonFallingSwords()
    {
        StartCoroutine(SummonSwordsCoroutine());
    }

    private IEnumerator SummonSwordsCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            if (player != null)
            {
                GameObject swordPrefab = swordPrefabs[i % swordPrefabs.Length];

                Vector3 spawnPos = player.transform.position + new Vector3(Random.Range(-1f, 1f), 5f, 0);
                GameObject sword = Instantiate(swordPrefab, spawnPos, swordPrefab.transform.rotation);

                FallingSwordController controller = sword.GetComponent<FallingSwordController>();
                if (controller != null)
                {
                    controller.Initialize(player.transform.position);
                }

                yield return new WaitUntil(() => sword == null);
            }
        }
    }

    private void ChooseRandomSkill()
    {
        int randomSkill = Random.Range(1, 4);
        switch (randomSkill)
        {
            case 1:
                Heal(hpValue);
                break;
            case 2:
                SpawnMiniEnemy();
                break;
            case 3:
                animator.SetTrigger("Teleport");
                if (audioSource != null && teleportSound != null)
                {
                    audioSource.PlayOneShot(teleportSound);
                }
                Teleport();
                break;
            case 4:
                SummonFallingSwords();
                break;
        }
    }

    private void UseSkill()
    {
        float currentCooldown = enraged ? skillCooldown / 2f : skillCooldown; 

        if (Time.time >= nextSkill)
        {
            nextSkill = Time.time + currentCooldown;
            ChooseRandomSkill();
            if (enraged)
            {
                ChooseRandomSkill(); 
            }
        }
    }

    protected override void Die()
    {
        animator.SetTrigger("Death");
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
        base.Die();
    }
}
using UnityEngine;

public class BossEnemyController : EnemyController
{
    [SerializeField] private GameObject bulletPrefabs;
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
    

    protected override void Update()
    {
        base.Update();

        NormalAttack();
        UseSkill();
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

    private void NormalAttack()
    {
        if (player != null && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
            EnemyBulletController enemyBullet = bullet.AddComponent<EnemyBulletController>();
            enemyBullet.SetMovementDirection(directionToPlayer * normalAttackSpeed);
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

    private void ChooseRandomSkill()
    {
        int randomSkill = Random.Range(0, 3);
        switch (randomSkill)
        {
            case 0:
                SpecialAttack();
                break;
            case 1:
                Heal(hpValue);
                break;
            case 2:
                SpawnMiniEnemy();
                break;
            case 3:
                Teleport();
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
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
        base.Die();
    }
}

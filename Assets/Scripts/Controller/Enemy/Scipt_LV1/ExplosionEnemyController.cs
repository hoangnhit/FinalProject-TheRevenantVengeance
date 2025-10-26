using UnityEngine;

public class ExplosionEnemyController : EnemyController
{
    [SerializeField] private GameObject miniEnemyPrefab;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private float spawnCooldown = 8f;
    [SerializeField] private float attackDistance = 6f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip takeHitSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip spawnSound;  // Thêm âm thanh khi spawn mini enemy (n?u mu?n)

    private float cooldownTimer = 0f;
    private Animator animator;
    private bool isAttacking = false;
    //protected bool isDead = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (IsDead()) return;
        base.Update();

        cooldownTimer -= Time.deltaTime;

        if (player != null && !isAttacking)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= attackDistance && cooldownTimer <= 0f)
            {
                Attack();
                cooldownTimer = spawnCooldown;
            }
        }
    }

    protected override void MoveToPlayer()
    {
        if (player != null && !isAttacking && !IsDead())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > attackDistance * 0.8f)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
            }
            FlipEnemy();
        }
    }

    private void Attack()
    {
        if (IsDead()) return;
        isAttacking = true;
        animator?.SetTrigger("Attack");

        // Phát âm thanh Attack
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        Invoke(nameof(SpawnMiniEnemies), 0.5f);
        Invoke(nameof(ResetAttack), spawnCooldown);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void SpawnMiniEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject mini = Instantiate(miniEnemyPrefab, transform.position, Quaternion.identity);
            mini.transform.localScale = new Vector3(0.15f, 0.15f, 1f);

            MiniEnemyController miniScript = mini.GetComponent<MiniEnemyController>();
            if (miniScript != null)
            {
                miniScript.SetAsMiniEnemy();
            }
        }

        // Phát âm thanh spawn mini enemy (n?u có)
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }

    public override void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage, knockbackDirection);
        if (currentHp > 0)
        {
            animator?.SetTrigger("TakeHit");

            // Phát âm thanh Take Hit
            if (takeHitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(takeHitSound);
            }
        }
    }

    protected override void Die()
    {
        //isDead = true;
        animator?.SetTrigger("Die");

        // Phát âm thanh Die
        if (dieSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dieSound);
        }
        base.Die();
        //Destroy(gameObject, 2f);
    }
}

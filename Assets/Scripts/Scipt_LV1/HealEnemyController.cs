using UnityEngine;

public class HealEnemyController : EnemyController
{
    [SerializeField] private GameObject heartObject;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float healValue = 20f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float fireSpeed = 5f;
    [SerializeField] private float stoppingDistance = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;     // Âm thanh bắn
    [SerializeField] private AudioClip takeHitSound;    // Âm thanh khi bị đánh
    [SerializeField] private AudioClip dieSound;        // Âm thanh khi chết
    [SerializeField] private AudioClip fireballSound;   // Âm thanh fireball bay (nếu muốn)

    private float fireCooldown;
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

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= stoppingDistance && !isAttacking)
            {
                fireCooldown -= Time.deltaTime;
                if (fireCooldown <= 0f)
                {
                    Attack();
                    fireCooldown = fireRate;
                }
            }
        }
    }

    protected override void MoveToPlayer()
    {
        if (player != null && !isAttacking && !IsDead())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > stoppingDistance)
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

        // Phát âm thanh Attack (nếu có)
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        Invoke(nameof(ShootFireball), 0.3f);
        Invoke(nameof(ResetAttack), fireRate);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void ShootFireball()
    {
        if (fireballPrefab != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * fireSpeed;
            }

            // Phát âm thanh fireball (nếu có)
            if (fireballSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(fireballSound);
            }
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

        if (heartObject != null)
        {
            GameObject heart = Instantiate(heartObject, transform.position, Quaternion.identity);
            Destroy(heart, 5f);
        }
        base.Die();
        //Destroy(gameObject, 2f);
    }
}
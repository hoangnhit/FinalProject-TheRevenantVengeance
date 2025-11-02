//using UnityEngine;

//public class EnergyEnemyController : EnemyController
//{
//    [SerializeField] private GameObject energyObject;
//    [SerializeField] private float attackCooldown = 1f;
//    [SerializeField] private float attackDistance = 1.5f;

//    [Header("Audio Settings")]
//    [SerializeField] private AudioSource audioSource;
//    [SerializeField] private AudioClip attackSound;
//    [SerializeField] private AudioClip takeHitSound;
//    [SerializeField] private AudioClip dieSound;

//    private Animator animator;
//    private bool isAttacking = false;
//    private float lastAttackTime = 0f;
//    //protected bool isDead = false;

//    protected override void Awake()
//    {
//        base.Awake();
//        animator = GetComponent<Animator>();
//    }

//    protected override void Update()
//    {
//        if (IsDead()) return;
//        base.Update();

//        if (player != null && !isAttacking)
//        {
//            float distance = Vector2.Distance(transform.position, player.transform.position);
//            if (distance < attackDistance && Time.time - lastAttackTime > attackCooldown)
//            {
//                Attack();
//            }
//        }
//    }

//    protected override void MoveToPlayer()
//    {
//        if (player != null && !isAttacking && !IsDead())
//        {
//            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
//            if (distanceToPlayer > attackDistance * 0.8f)
//            {
//                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
//            }
//            FlipEnemy();
//        }
//    }

//    private void Attack()
//    {
//        if (IsDead()) return;
//        isAttacking = true;
//        lastAttackTime = Time.time;
//        animator?.SetTrigger("Attack");

//        if (attackSound != null && audioSource != null)
//        {
//            audioSource.PlayOneShot(attackSound);
//        }

//        // Thay vì reset ngay b?ng attackCooldown, t?ng thêm 0.3s (tùy theo animation)
//        Invoke(nameof(ResetAttack), attackCooldown + 0.3f);
//    }

//    private void ResetAttack()
//    {
//        isAttacking = false;
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player") && !IsDead())
//        {
//            player.TakeDamage(enterDamage);
//        }
//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player") && !IsDead() && isAttacking)
//        {
//            player.TakeDamage(stayDamage);
//        }
//    }

//    public override void TakeDamage(float damage, Vector2 knockbackDirection)
//    {
//        base.TakeDamage(damage, knockbackDirection);
//        if (currentHp > 0)
//        {
//            animator?.SetTrigger("TakeHit");

//            // Phát âm thanh Take Hit
//            if (takeHitSound != null && audioSource != null)
//            {
//                audioSource.PlayOneShot(takeHitSound);
//            }
//        }
//    }

//    protected override void Die()
//    {
//        //isDead = true;
//        animator?.SetTrigger("Die");

//        // Phát âm thanh Die
//        if (dieSound != null && audioSource != null)
//        {
//            audioSource.PlayOneShot(dieSound);
//        }

//        if (energyObject != null)
//        {
//            GameObject energy = Instantiate(energyObject, transform.position, Quaternion.identity);
//            Destroy(energy, 5f);
//        }
//        base.Die();
//        //Destroy(gameObject, 2f);
//    }
//}
using UnityEngine;

public class EnergyEnemyController : EnemyController
{
    [SerializeField] private GameObject energyObject;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDistance = 1.5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip takeHitSound;
    [SerializeField] private AudioClip dieSound;

    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (IsDead()) return;
        base.Update();

        if (player != null && !isAttacking)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < attackDistance && Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
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
        lastAttackTime = Time.time;

        animator?.SetTrigger("Attack");

        // Play attack sound
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Gây damage sau 0.3s (phù h?p v?i frame animation)
        Invoke(nameof(DealDamageToPlayer), 0.3f);

        // Reset attack sau cooldown
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void DealDamageToPlayer()
    {
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < attackDistance)
        {
            player.TakeDamage(enterDamage);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public override void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage, knockbackDirection);
        if (currentHp > 0)
        {
            animator?.SetTrigger("TakeHit");

            if (takeHitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(takeHitSound);
            }
        }
    }

    protected override void Die()
    {
        animator?.SetTrigger("Die");

        if (dieSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dieSound);
        }

        if (energyObject != null)
        {
            GameObject energy = Instantiate(energyObject, transform.position, Quaternion.identity);
            Destroy(energy, 5f);
        }

        base.Die();
    }
}

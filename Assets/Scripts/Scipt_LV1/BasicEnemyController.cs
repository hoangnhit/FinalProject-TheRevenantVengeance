 //using UnityEngine;

//public class BasicEnemyController : EnemyController
//{
//	private Animator animator;
//	private bool isAttacking = false;
//	private float attackCooldown = 1f;
//	private float lastAttackTime = 0f;
//	protected bool isDead = false;
//	protected override void Awake()
//	{
//		base.Awake();
//		animator = GetComponent<Animator>();
//	}

//	protected override void Update()
//	{
//		if (isDead) return;
//		base.Update();
//		if (player != null && !isAttacking)
//		{
//			float distance = Vector2.Distance(transform.position, player.transform.position);
//			if (distance < 1.5f && Time.time - lastAttackTime > attackCooldown)
//			{
//				Attack();
//			}
//		}
//	}

//	private void Attack()
//	{
//		if (isDead) return;
//		isAttacking = true;
//		lastAttackTime = Time.time;

//		animator?.SetTrigger("Attack");

//		// Gây sát th??ng sau 0.5s (tùy theo frame animation)
//		Invoke(nameof(DealDamageToPlayer), 0.3f);

//		// Cho phép attack l?i sau cooldown
//		Invoke(nameof(ResetAttack), attackCooldown);
//	}

//	private void DealDamageToPlayer()
//	{
//		if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
//		{
//			player.TakeDamage(enterDamage); // Ho?c attackDamage riêng
//		}
//	}

//	private void ResetAttack()
//	{
//		isAttacking = false;
//	}

//	public override void TakeDamage(float damage, Vector2 knockbackDirection)
//	{
//		base.TakeDamage(damage, knockbackDirection);
//		if (currentHp > 0)
//		{
//			animator?.SetTrigger("TakeHit");
//		}
//	}

//	protected override void Die()
//	{
//		isDead = true;
//		animator?.SetTrigger("Die");
//		Destroy(gameObject, 2f);
//	}

//	//private void OnTriggerEnter2D(Collider2D collision)
//	//{
//	//    if (collision.CompareTag("Player"))
//	//    {
//	//        player.TakeDamage(enterDamage);
//	//    }
//	//}

//	//private void OnTriggerStay2D(Collider2D collision)
//	//{
//	//    if (collision.CompareTag("Player"))
//	//    {
//	//        player.TakeDamage(stayDamage);
//	//    }
//	//}
//}

using UnityEngine;

public class BasicEnemyController : EnemyController
{
	private Animator animator;
	private bool isAttacking = false;
	private float attackCooldown = 1f;
	private float lastAttackTime = 0f;
	//protected bool isDead = false;

	[Header("Audio Settings")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip attackSound;
	[SerializeField] private AudioClip takeHitSound;
	[SerializeField] private AudioClip dieSound;

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
			if (distance < 1.5f && Time.time - lastAttackTime > attackCooldown)
			{
				Attack();
			}
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

		Invoke(nameof(DealDamageToPlayer), 0.3f);
		Invoke(nameof(ResetAttack), attackCooldown);
	}

	private void DealDamageToPlayer()
	{
		if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
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
			// Play take hit sound
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
		// Play die sound
		if (dieSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(dieSound);
		}
		base.Die();
		//Destroy(gameObject, 2f);
	}
}

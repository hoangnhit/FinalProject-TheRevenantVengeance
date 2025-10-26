using UnityEngine;

public class MiniEnemyController : EnemyController
{
	[SerializeField] private float moveSpeed = 3f;
	[SerializeField] private float lifeTime = 5f; // Th?i gian t?n t?i t?i ?a (5 giây)

	private float timer = 0f;

	protected override void Update()
	{
		//base.Update();

		// Di chuy?n dí theo Player
		if (player != null)
		{
			Vector2 direction = (player.transform.position - transform.position).normalized;
			transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
		}

		// ??m th?i gian s?ng
		timer += Time.deltaTime;
		if (timer >= lifeTime)
		{
			Destroy(gameObject); // T? hu? sau 5 giây
		}
	}

	public void SetAsMiniEnemy()
	{
		// KHÔNG c?n set scale n?a, vì ?ã set lúc spawn

		// Gi?m máu còn 1 ph?n (ví d? 25% máu g?c)
		maxHp *= 0.25f;
		currentHp = maxHp;

		// Gi?m sát th??ng ho?c t?c ?? n?u c?n (tu? b?n)
		enemySpeed = moveSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			player.TakeDamage(enterDamage);
		}
	}
}

using UnityEngine;

public class ExplosionController : MonoBehaviour
{
	[SerializeField] private float damage = 30f;
	// --- THÊM BI?N CHO L?C ??Y LÙI C?A V? N? ---
	[SerializeField] private float explosionKnockbackForce = 10f; // L?c ??y lùi khi b? n?

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// X? lý sát th??ng cho Player (ch?a c?n knockback cho Player)
		PlayerController player = collision.GetComponent<PlayerController>();
		if (player != null && collision.CompareTag("Player")) // Ki?m tra null và tag
		{
			player.TakeDamage(damage);
		}

		// X? lý sát th??ng và ??y lùi cho Enemy
		EnemyController enemy = collision.GetComponent<EnemyController>();
		if (enemy != null && collision.CompareTag("Enemy")) // Ki?m tra null và tag
		{
			// Tính toán h??ng ??y lùi t? tâm v? n? ??n Enemy
			Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
			// G?i TakeDamage v?i sát th??ng và h??ng ??y lùi
			enemy.TakeDamage(damage, knockbackDirection * explosionKnockbackForce); // Truy?n damage và h??ng ??y lùi
																					// L?u ý: hàm TakeDamage nh?n m?t Vector2 làm tham s? th? hai, ??i di?n cho h??ng.
																					// knockbackDirection.normalized * explosionKnockbackForce có th? ???c s? d?ng làm tham s? th? hai
																					// ho?c b?n có th? ch? truy?n knockbackDirection và ?? enemy t? nhân v?i knockbackForce c?a nó.
																					// Tôi ?ã s?a EnemyController.TakeDamage ?? ch? nh?n h??ng, và EnemyController.ApplyKnockback s? nhân v?i knockbackForce.
																					// V?y nên, ch? c?n truy?n knockbackDirection.
		}
	}

	public void DestroyExplosion()
	{
		Destroy(gameObject);
	}
}

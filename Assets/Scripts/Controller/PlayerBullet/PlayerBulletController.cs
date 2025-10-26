using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBulletController : MonoBehaviour
{
	//[SerializeField] private float moveSpeed = 25f;
	//[SerializeField] private float destroyTime = 0.5f;
	//[SerializeField] private float damage = 10f;
	//[SerializeField] GameObject bloodPrefab;

	//void Start()
	//{
	//    Destroy(gameObject, destroyTime);
	//}


	//void Update()
	//{
	//    BulletMovement();
	//}

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//    if (collision.CompareTag("Enemy"))
	//    {
	//        EnemyController enemy = collision.GetComponent<EnemyController>();
	//        if (enemy != null)
	//        {
	//            enemy.TakeDamage(damage);
	//            GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
	//            Destroy(blood, 1f);
	//        }
	//        Destroy(gameObject);
	//    }
	//}

	//void BulletMovement()
	//{
	//    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
	//}

	[SerializeField] private float moveSpeed = 25f;
	[SerializeField] private float destroyTime = 0.5f;
	[SerializeField] private float damage = 10f;
	[SerializeField] GameObject bloodPrefab;

	void Start()
	{
		Destroy(gameObject, destroyTime);
	}

	void Update()
	{
		BulletMovement();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			EnemyController enemy = collision.GetComponent<EnemyController>();
			if (enemy != null)
			{
				// --- THAY ??I DÒNG NÀY ?? THÊM H??NG ??Y LÙI ---
				// Tính toán h??ng ??y lùi t? viên ??n ??n Enemy
				Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

				// G?i TakeDamage v?i sát th??ng và h??ng ??y lùi
				enemy.TakeDamage(damage, knockbackDirection);
				// EnemyController.TakeDamage ?ã nh?n Vector2 và t? nhân v?i knockbackForce riêng c?a enemy.

				// T?o hi?u ?ng máu
				GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
				Destroy(blood, 1f);
			}
			Destroy(gameObject); // H?y viên ??n sau khi va ch?m
		}
	}

	void BulletMovement()
	{
		transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
	}
}

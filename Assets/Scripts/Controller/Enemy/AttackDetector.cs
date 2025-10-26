//using UnityEngine;

//public class AttackDetector : MonoBehaviour
//{
//	[SerializeField] private float attackDamage = 10f; // Sát th??ng c?a ?òn chém

//	private void OnTriggerEnter2D(Collider2D collision)
//	{
//		// Ki?m tra xem ??i t??ng va ch?m có Tag "Enemy" không
//		//if (collision.CompareTag("Enemy"))
//		//{
//		//	// L?y component EnemyController t? ??i t??ng va ch?m
//		//	// Vì t?t c? các k? ??ch c?a b?n ??u k? th?a/là EnemyController, chúng ta có th? l?y component này.
//		//	EnemyController enemy = collision.GetComponent<EnemyController>();

//		//	if (enemy != null)
//		//	{
//		//		// G?i hàm TakeDamage trên ??i t??ng Enemy
//		//		enemy.TakeDamage(attackDamage);
//		//	}
//		//	// else
//		//	// {
//		//	//     Debug.LogWarning($"Enemy with tag 'Enemy' on {collision.gameObject.name} does not have an EnemyController component!");
//		//	// }
//		//}
//		if (collision.CompareTag("Enemy"))
//		{
//			// L?y component EnemyController t? ??i t??ng va ch?m
//			// Dù là HealEnemyController hay BasicEnemyController, n?u chúng k? th?a t? EnemyController,
//			// thì GetComponent<EnemyController>() v?n s? tr? v? ?úng component c?a l?p con.
//			EnemyController enemy = collision.GetComponent<EnemyController>();

//			if (enemy != null)
//			{
//				// --- TÍNH TOÁN H??NG ??Y LÙI ---
//				// L?y h??ng mà Player ?ang quay m?t ?? ??y lùi k? ??ch
//				// Gi? ??nh GameObject cha c?a AttackHitbox là Player
//				Vector2 knockbackDirection;
//				if (transform.parent.localScale.x < 0) // Player quay m?t sang trái
//				{
//					knockbackDirection = new Vector2(-1, 0); // ??y k? ??ch sang trái
//				}
//				else // Player quay m?t sang ph?i
//				{
//					knockbackDirection = new Vector2(1, 0); // ??y k? ??ch sang ph?i
//				}

//				// Tùy ch?n: N?u b?n mu?n ??y lùi có thêm m?t chút "n?y" lên trên
//				// knockbackDirection = new Vector2(knockbackDirection.x, 0.5f).normalized; 

//				// G?i hàm TakeDamage trên ??i t??ng Enemy, truy?n sát th??ng và h??ng ??y lùi
//				enemy.TakeDamage(attackDamage, knockbackDirection);
//			}
//			// else
//			// {
//			//     Debug.LogWarning($"Enemy with tag 'Enemy' on {collision.gameObject.name} does not have an EnemyController component!", collision.gameObject);
//			// }
//		}
//	}
//}
using UnityEngine;
using TMPro;

public class AttackDetector : MonoBehaviour
{
	[SerializeField] public float attackDamage = 10f;
    [SerializeField] private GameObject damageTextPrefab; // Prefab ?ã t?o


    private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log($"Va ch?m v?i: {collision.name} (Tag: {collision.tag})");

		if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
		{
			Debug.Log("Enemy ho?c Boss b? chém!");

			EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector2 knockbackDir = transform.parent.localScale.x < 0 ? Vector2.left : Vector2.right;
                enemy.TakeDamage(attackDamage, knockbackDir);

                // ?? Hi?n th? s? damage
                if (damageTextPrefab != null)
                {
                    Vector3 spawnPos = collision.transform.position + new Vector3(0, 1, 0); // h?i trên ??u enemy
                    GameObject textObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

                    DamageText dmgText = textObj.GetComponent<DamageText>();
                    if (dmgText != null)
                    {
                        dmgText.SetDamage(attackDamage);
                    }
                }
            }
            else
			{
				Debug.LogWarning("Không tìm th?y EnemyController trong " + collision.name);
			}
		}
	}

}

using UnityEngine;

public class FireBallFly : MonoBehaviour
{
	[SerializeField] private float damage = 10f;
	[SerializeField] private float lifeTime = 5f;

	private void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController player = collision.GetComponent<PlayerController>();
			if (player != null)
			{
				player.TakeDamage(damage);
			}

			Destroy(gameObject);
		}
		else if (collision.CompareTag("Rock")) // N?u ??ng t??ng, c?ng hu? ??n
		{
			Destroy(gameObject);
		}
	}
}

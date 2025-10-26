using UnityEngine;

public class EnergyItemController : MonoBehaviour
{
    [SerializeField] private float energyValue = 20f;
    [SerializeField] private float attractRange = 3f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Âm thanh thu thập Energy")]
    [SerializeField] private AudioClip collectEnergyClip;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attractRange)
        {
            // Di chuyển về phía player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GetEnergy(energyValue);
                if (collectEnergyClip != null)
                {
                    AudioSource.PlayClipAtPoint(collectEnergyClip, transform.position);
                }
                Destroy(gameObject);
            }
        }
    }
}

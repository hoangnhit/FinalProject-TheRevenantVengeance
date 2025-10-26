using UnityEngine;

public class ExpController : MonoBehaviour
{
    [SerializeField] private float expValue = 10f;
    [SerializeField] private float attractRange = 3f;   
    [SerializeField] private float moveSpeed = 5f;

    [Header("Âm thanh thu thập EXP")]
    [SerializeField] private AudioClip collectExpClip;
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
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GetExp(expValue);
                if (collectExpClip != null)
                {
                    AudioSource.PlayClipAtPoint(collectExpClip, transform.position);
                }
                Destroy(gameObject);
            }
        }
    }
}

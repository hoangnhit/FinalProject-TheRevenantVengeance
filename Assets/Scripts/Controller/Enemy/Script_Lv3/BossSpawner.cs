using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;

    private bool bossSpawned = false;
    private float spawnTime = 10f;

    void Update()
    {
        if (!bossSpawned && Time.timeSinceLevelLoad >= spawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    private void SpawnBoss()
    {
        Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Boss spawned after 2 minutes!");
    }
}

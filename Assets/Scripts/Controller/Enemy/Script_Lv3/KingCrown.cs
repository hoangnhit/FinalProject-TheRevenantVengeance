using UnityEngine;
using UnityEngine.SceneManagement;

public class KingCrown : MonoBehaviour
{
    [SerializeField] private string endSceneName = "EndGameScene"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(endSceneName);
        }
    }
}

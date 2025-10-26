using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class UniversalGateTrigger : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private Tilemap gateTilemap;
    [SerializeField] private Collider2D gateCollider;
    [SerializeField] private TMP_Text gateMessageText;
    [SerializeField] private AudioSource gateOpenSound;

    [Header("Energy Settings")]
    [SerializeField] private bool requireEnergyToOpen = false;
    [SerializeField] private int requiredEnergy = 10;
    [SerializeField] private PlayerController player;

    [Header("Scene Settings")]
    [SerializeField] private bool loadNextScene = true;


    private bool gateOpened = false;

    private void Update()
    {
        if (!gateOpened && requireEnergyToOpen && player != null && player.GetCurrentEnergy() >= requiredEnergy)
        {
            OpenGate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gateOpened && requireEnergyToOpen && collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && player.GetCurrentEnergy() >= requiredEnergy)
            {
                OpenGate();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gateOpened && collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (loadNextScene)
                {
                    int currentIndex = SceneManager.GetActiveScene().buildIndex;
                    if (currentIndex + 1 < SceneManager.sceneCountInBuildSettings)
                    {
                        Debug.Log("Loading next scene...");
                        SceneManager.LoadScene(currentIndex + 1);
                    }
                    else
                    {
                        Debug.LogWarning("No next scene found in build settings!");
                    }
                }
            }
        }
    }


    public void OpenGate()
    {
        if (gateOpened) return;

        gateOpened = true;

        if (gateTilemap != null)
            gateTilemap.gameObject.SetActive(false);

        if (gateCollider != null)
            gateCollider.enabled = false; 

        if (gateOpenSound != null)
            gateOpenSound.Play();

        if (gateMessageText != null)
        {
            gateMessageText.text = "The Gate is Open!";
            Invoke(nameof(HideMessage), 2f);
        }
    }

    private void HideMessage()
    {
        if (gateMessageText != null)
            gateMessageText.text = "";
    }
}

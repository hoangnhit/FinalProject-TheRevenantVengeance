using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class GateTriggerBossLv3 : MonoBehaviour
{
    [SerializeField] private Tilemap gateTilemap;
    //[SerializeField] private TMP_Text gateMessageText;
    [SerializeField] private AudioSource gateOpenSound;
    [SerializeField] private Collider2D gateCollider;

    [Header("Scene Transition Settings")]
    [SerializeField] private string targetSceneName;

    private bool gateOpened = false;

    public void OpenGate()
    {
        if (!gateOpened)
        {
            gateOpened = true;

            if (gateTilemap != null)
            {
                gateTilemap.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Gate Tilemap is NULL.");
            }

            if (gateOpenSound != null)
            {
                gateOpenSound.Play();
            }

            //if (gateMessageText != null)
            //{
            //    gateMessageText.text = "The Gate is Open!";
            //    Invoke(nameof(HideMessage), 2f);
            //}
        }
    }

    //private void HideMessage()
    //{
    //    if (gateMessageText != null)
    //        gateMessageText.text = "";
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gateOpened && collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!string.IsNullOrEmpty(targetSceneName))
                {
                    SceneManager.LoadScene(targetSceneName);
                }
                else
                {
                    Debug.LogWarning("Target Scene Name is not set in GateTriggerBossLv3.");
                }
            }
        }
    }
}

//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.SceneManagement;

//public class GateTrigger : MonoBehaviour
//{
//	[SerializeField] private Tilemap gateTilemap;  // GÁN Tilemap Gate ? ?ây
//	[SerializeField] private int requiredEnergy = 10;

//	private bool gateOpened = false;

//	private void OnTriggerEnter2D(Collider2D collision)
//	{
//		if (collision.CompareTag("Player"))
//		{
//			PlayerController player = collision.GetComponent<PlayerController>();
//			if (player != null && player.GetCurrentEnergy() >= requiredEnergy)
//			{
//				OpenGate();
//			}
//		}
//	}

//	private void OpenGate()
//	{
//		if (gateTilemap != null && !gateOpened)
//		{
//			gateTilemap.gameObject.SetActive(false);  // T?t c?ng
//			gateOpened = true;
//		}
//	}

//	private void OnTriggerStay2D(Collider2D collision)
//	{
//		if (gateOpened && collision.CompareTag("Player"))
//		{
//			if (Input.GetKeyDown(KeyCode.W))  // Nh?n W ?? vào map m?i
//			{
//				SceneManager.LoadScene("NextSceneName");  // ??i tên Scene t?i ?ây
//			}
//		}
//	}
//}
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class GateTrigger : MonoBehaviour
//{
//	[SerializeField] private Tilemap gateTilemap;
//	[SerializeField] private int requiredEnergy = 10;
//	[SerializeField] private Text gateMessageText;  // Gán UI Text thông báo
//	[SerializeField] private AudioSource gateOpenSound;  // Gán AudioSource ch?a âm m? c?ng

//	private bool gateOpened = false;

//	private void OnTriggerEnter2D(Collider2D collision)
//	{
//		if (collision.CompareTag("Player"))
//		{
//			PlayerController player = collision.GetComponent<PlayerController>();
//			if (player != null && player.GetCurrentEnergy() >= requiredEnergy)
//			{
//				OpenGate();
//			}
//		}
//	}

//	private void OpenGate()
//	{
//		if (!gateOpened)
//		{
//			gateTilemap.gameObject.SetActive(false);
//			gateOpened = true;

//			// Phát âm thanh m? c?ng
//			if (gateOpenSound != null)
//			{
//				gateOpenSound.Play();
//			}

//			// Hi?n th? thông báo c?ng m?
//			if (gateMessageText != null)
//			{
//				gateMessageText.text = "The Gate is Open!";
//				Invoke(nameof(HideMessage), 2f);  // ?n sau 2 giây (tùy ch?nh)
//			}
//		}
//	}

//	private void HideMessage()
//	{
//		if (gateMessageText != null)
//		{
//			gateMessageText.text = "";
//		}
//	}

//	private void OnTriggerStay2D(Collider2D collision)
//	{
//		if (gateOpened && collision.CompareTag("Player"))
//		{
//			if (Input.GetKeyDown(KeyCode.W))
//			{
//				SceneManager.LoadScene("SceneLv1.2");
//			}
//		}
//	}
//}
using Assets.Scripts.Controller;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GateTrigger : MonoBehaviour
{
	[SerializeField] private Tilemap gateTilemap;
	[SerializeField] private int requiredEnergy = 10;
	[SerializeField] private TMP_Text gateMessageText;  // DÙNG TMP_Text
	[SerializeField] private AudioSource gateOpenSound;
	[SerializeField] private PlayerController player;
	[SerializeField] private Collider2D gateCollider;  // THÊM: Kéo Collider c?a c?ng vào ?ây (Composite Collider ho?c Tilemap Collider 2D)
	[SerializeField] private int sceneIndexToLoad = 5;

    private bool gateOpened = false;

	private void Update()
	{
		if (!gateOpened && player != null && player.GetCurrentEnergy() >= requiredEnergy)
		{
			OpenGate();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController player = collision.GetComponent<PlayerController>();
			if (player != null && player.GetCurrentEnergy() >= requiredEnergy)
			{
				OpenGate();
			}
		}
	}

	private void OpenGate()
	{
		if (!gateOpened)
		{
			gateOpened = true;

			// ?n Tilemap (?n hình ?nh c?ng)
			if (gateTilemap != null)
			{
				gateTilemap.gameObject.SetActive(false);
			}

			// T?t collider ?? Player ?i qua
			if (gateCollider != null)
			{
				gateCollider.enabled = false;
			}

			// Phát âm thanh m? c?ng
			if (gateOpenSound != null)
			{
				gateOpenSound.Play();
			}

			// Hi?n thông báo
			if (gateMessageText != null)
			{
				gateMessageText.text = "The Gate is Open!";
				Invoke(nameof(HideMessage), 2f);
			}
		}
	}

	private void HideMessage()
	{
		if (gateMessageText != null)
		{
			gateMessageText.text = "";
		}
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gateOpened && collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                StartCoroutine(SaveAndLoadScene(pc));
            }
        }
    }

    private IEnumerator SaveAndLoadScene(PlayerController pc)
    {
        PlayerState.Level = pc.level;
        PlayerState.MaxHp = pc.maxHp;
        PlayerState.CurrentHp = pc.currentHp;
        PlayerState.MaxExp = pc.maxExp;
        PlayerState.CurrentExp = pc.currentExp;
        PlayerState.MoveSpeed = pc.moveSpeed;
        PlayerState.NormalDamge = pc.attackDetector.attackDamage;
        //PlayerState.Skill1Damge = pc.fireBall.damage;
        //PlayerState.Skill2Damge = pc.swordSpin.damage;

        if (pc.fireBall != null)
        {
            PlayerState.Skill1Damge = pc.fireBall.damage;
        }     

        if (pc.swordSpin != null)
        {
            PlayerState.Skill2Damge = pc.swordSpin.damage;
        }

        Debug.Log("Save statistic");
        yield return null; // đợi 1 frame để chắc chắn dữ liệu lưu xong

        SceneManager.LoadScene(sceneIndexToLoad);
    }
}


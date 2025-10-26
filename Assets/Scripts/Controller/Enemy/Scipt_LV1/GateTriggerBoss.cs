////using UnityEngine;
////using UnityEngine.Tilemaps;
////using TMPro;
////using UnityEngine.SceneManagement;

////public class GateTriggerBoss : MonoBehaviour
////{
////	[SerializeField] private Tilemap gateTilemap;
////	[SerializeField] private TMP_Text gateMessageText;
////	[SerializeField] private AudioSource gateOpenSound;
////	[SerializeField] private Collider2D gateCollider;

////	private bool gateOpened = false;

////	public void OpenGate()
////	{
////		Debug.Log("?ã g?i OpenGate()");

////		if (!gateOpened)
////		{
////			Debug.Log("?ang m? c?ng...");

////			gateOpened = true;

////			if (gateTilemap != null)
////			{
////				Debug.Log("?n Tilemap thành công.");
////				gateTilemap.gameObject.SetActive(false);
////			}
////			else
////			{
////				Debug.LogWarning("gateTilemap NULL.");
////			}

////			if (gateCollider != null)
////			{
////				Debug.Log("T?t Collider thành công.");
////				gateCollider.enabled = false;
////			}
////			else
////			{
////				Debug.LogWarning("gateCollider NULL.");
////			}

////			if (gateOpenSound != null)
////			{
////				gateOpenSound.Play();
////			}

////			if (gateMessageText != null)
////			{
////				gateMessageText.text = "The Gate is Open!";
////				Invoke(nameof(HideMessage), 2f);
////			}
////		}
////		else
////		{
////			Debug.Log("C?ng ?ã m? t? tr??c, không m? l?i.");
////		}
////	}

////	private void HideMessage()
////	{
////		if (gateMessageText != null)
////		{
////			gateMessageText.text = "";
////		}
////	}

////	private void OnTriggerStay2D(Collider2D collision)
////	{
////		if (gateOpened && collision.CompareTag("Player"))
////		{
////			if (Input.GetKeyDown(KeyCode.W))
////			{
////				SceneManager.LoadScene("SceneLv1.23");
////			}
////		}
////	}
////}
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class GateTriggerBoss : MonoBehaviour
//{
//	[SerializeField] private Tilemap gateTilemap;
//	[SerializeField] private TMP_Text gateMessageText;
//	[SerializeField] private AudioSource gateOpenSound;
//	[SerializeField] private Collider2D gateCollider; // ??m b?o b?n ?ã gán Collider2D c?a c?ng vào ?ây trong Inspector

//	private bool gateOpened = false;

//	public void OpenGate()
//	{
//		Debug.Log("?ã g?i OpenGate()"); // ?ã s?a l?i font ti?ng Vi?t

//		if (!gateOpened)
//		{
//			Debug.Log("?ang m? c?ng..."); // ?ã s?a l?i font ti?ng Vi?t

//			gateOpened = true;

//			if (gateTilemap != null)
//			{
//				Debug.Log("?n Tilemap thành công."); // ?ã s?a l?i font ti?ng Vi?t
//				gateTilemap.gameObject.SetActive(false); // ?n hình ?nh c?ng
//			}
//			else
//			{
//				Debug.LogWarning("gateTilemap NULL.");
//			}

//			// *** DÒNG NÀY ?Ã ???C XÓA HO?C COMMENT L?I ***
//			// *** KHÔNG T?T gateCollider N?A ***
//			// N?u dòng d??i ?ây v?n còn, b?n s? không th? chuy?n scene.
//			// if (gateCollider != null)
//			// {
//			//     Debug.Log("T?t Collider thành công.");
//			//     gateCollider.enabled = false; // <<< DÒNG CÓ V?N ?? NÀY ?Ã B? XÓA/COMMENT!
//			// }
//			// else
//			// {
//			//     Debug.LogWarning("gateCollider NULL.");
//			// }

//			if (gateOpenSound != null)
//			{
//				gateOpenSound.Play();
//			}

//			if (gateMessageText != null)
//			{
//				gateMessageText.text = "The Gate is Open!";
//				Invoke(nameof(HideMessage), 2f);
//			}
//		}
//		else
//		{
//			Debug.Log("C?ng ?ã m? t? tr??c, không m? l?i."); // ?ã s?a l?i font ti?ng Vi?t
//		}
//	}

//	private void HideMessage()
//	{
//		if (gateMessageText != null)
//			gateMessageText.text = "";
//	}

//	private void OnTriggerStay2D(Collider2D collision)
//	{
//		// Thêm Debug.Log ?? ki?m tra xem hàm này có ???c g?i không
//		Debug.Log("OnTriggerStay2D ?ang ???c ki?m tra.");

//		if (gateOpened && collision.CompareTag("Player"))
//		{
//			// Thêm Debug.Log ?? ki?m tra ?i?u ki?n này
//			Debug.Log("Ng??i ch?i trong vùng trigger VÀ c?ng ?ã m?.");
//			Debug.Log("Input.GetKeyDown(KeyCode.W) hi?n t?i: " + Input.GetKeyDown(KeyCode.W)); // THÊM DÒNG NÀY
//			if (Input.GetKeyDown(KeyCode.W)) // Dòng này ?ã t?n t?i
//			{
//				// Thêm Debug.Log ?? xác nh?n nút W ???c nh?n
//				Debug.Log("Ng??i ch?i b?m W! ?ang t?i scene SceneLv1.3...");
//				SceneManager.LoadScene("SceneLv1.3");
//			}
//		}
//	}
//}
// (Ph?n code c? ?ã comment, không quan tâm)

using Assets.Scripts.Controller;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GateTriggerBoss : MonoBehaviour
{
	[SerializeField] private Tilemap gateTilemap;
	//[SerializeField] private TMP_Text gateMessageText;
	[SerializeField] private AudioSource gateOpenSound;
	[SerializeField] private Collider2D gateCollider;


	// --- THÊM DÒNG NÀY VÀO ?ÂY ---
	[Header("Scene Transition Settings")] // Tiêu ?? trong Inspector
	[SerializeField] private string targetSceneName; // Bi?n ?? l?u tên c?nh ?ích

	private bool gateOpened = false;

	public void OpenGate()
	{
		Debug.Log("?ã g?i OpenGate()"); // ?ã s?a l?i font ti?ng Vi?t

		if (!gateOpened)
		{
			Debug.Log("?ang m? c?ng..."); // ?ã s?a l?i font ti?ng Vi?t

			gateOpened = true;

			if (gateTilemap != null)
			{
				Debug.Log("?n Tilemap thành công."); // ?ã s?a l?i font ti?ng Vi?t
				gateTilemap.gameObject.SetActive(false); // ?n hình ?nh c?ng
			}
			else
			{
				Debug.LogWarning("gateTilemap NULL.");
			}

			// *** KHÔNG CÓ DÒNG gateCollider.enabled = false; ? ?ÂY N?A ***
			// Vui lòng ??m b?o b?n ?ã xóa ho?c comment dòng này nh? ?ã h??ng d?n.

			if (gateOpenSound != null)
			{
				gateOpenSound.Play();
			}

			//if (gateMessageText != null)
			//{
			//	gateMessageText.text = "The Gate is Open!";
			//	Invoke(nameof(HideMessage), 2f);
			//}
		}
		else
		{
			Debug.Log("C?ng ?ã m? t? tr??c, không m? l?i."); // ?ã s?a l?i font ti?ng Vi?t
		}
	}

	//private void HideMessage()
	//{
	//	if (gateMessageText != null)
	//		gateMessageText.text = "";
	//}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Debug.Log("OnTriggerStay2D ?ang ???c ki?m tra.");

		if (gateOpened && collision.CompareTag("Player"))
		{

            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                StartCoroutine(SaveAndLoadScene(pc));
            }

            Debug.Log("Ng??i ch?i trong vùng trigger VÀ c?ng ?ã m?.");
			Debug.Log("Input.GetKeyDown(KeyCode.W) hi?n t?i: " + Input.GetKeyDown(KeyCode.W));
			//if (Input.GetKeyDown(KeyCode.W))
			//{
				// --- THAY ??I DÒNG NÀY ?? S? D?NG BI?N targetSceneName ---
				if (!string.IsNullOrEmpty(targetSceneName)) // ??m b?o tên c?nh không r?ng
				{
					Debug.Log("Ng??i ch?i b?m W! ?ang t?i scene: " + targetSceneName + "..."); // Log tên c?nh ?ích
                    //SceneTransitionManager.instance.TransitionToScene(targetSceneName);
					SceneManager.LoadScene(targetSceneName);
				}
            else
				{
					Debug.LogWarning("Target Scene Name ch?a ???c ??t trong Inspector c?a GateTriggerBoss!");
				}
			//}
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

    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatMenuController : MonoBehaviour
{
    [Header("Cheat Input")]
    public TMP_InputField cheatInputField;
    public Button continueButton;
    public Button closeButton;
    public PlayerController playerController;
    public GameStateManager gameStateManager;

    void Start()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(CheckCheatGame);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => gameStateManager.CloseCheatMenu());
    }

    public void CheckCheatGame()
    {
        string cheatCode = cheatInputField.text.Trim().ToLower();
        if (cheatCode == "godmode" || cheatCode == "kietoccho")
        {
            if (playerController != null)
            {
                for (int i = 0; i < 100; i++)
                {
                    playerController.LevelUp();
                }
                Debug.Log("God mode activated!");
            }
            else
            {
                Debug.LogError("PlayerController reference is not assigned!");
            }
        }
        else
        {
            Debug.LogWarning("Invalid cheat code entered.");
        }

        cheatInputField.text = string.Empty;
        gameStateManager.CloseCheatMenu();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu Buttons")]
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    void Start()
    {
        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() => GameStateManager.instance.ResumeGame());

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => GameStateManager.instance.RestartGame());

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => GameStateManager.instance.QuitGame());

    }
}

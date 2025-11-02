using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public GameObject pauseMenuUI;
    public GameObject cheatMenuUI;
    private bool isPaused = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //else if (instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isPaused)
                CloseCheatMenu();
            else
                OpenCheatMenu();
        }
    }


    public void ResumeGame()
    {
        //Debug.LogWarning("Resume Clicked.");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        MusicManager.Instance?.ResumeMusic();
    }

    public void PauseGame()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("Pause menu UI is still null.");
            return;
        }

        Debug.Log("Pause menu found. Activating...");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        MusicManager.Instance?.PauseMusic();
    }
    public void RestartGame()
    {
        Debug.LogWarning("Restart Clicked.");
        Time.timeScale = 1f;
        if (GameTimerManager.Instance != null)
        {
            GameTimerManager.Instance.ResetTimer();
            GameTimerManager.Instance.StartTimer();
        }

        if (MusicManager.Instance != null) MusicManager.Instance.RestartMusic();

        SceneManager.LoadScene(1);
    }
    public void QuitToStart()
    {
        Debug.LogWarning("Quit to Start Clicked.");
        Time.timeScale = 1f;
        if (GameTimerManager.Instance != null)
        {
            GameTimerManager.Instance.ResetTimer();
            GameTimerManager.Instance.StartTimer();
        }
        if (MusicManager.Instance != null) MusicManager.Instance.RestartMusic();
        SceneManager.LoadScene(0);
    }

    public void OpenCheatMenu()
    {
        Debug.LogWarning("Open Cheat Menu Clicked.");
        if (cheatMenuUI != null)
        {
            cheatMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            MusicManager.Instance?.PauseMusic();
        }
        else
        {
            Debug.LogWarning("Cheat menu UI is still null.");
        }
    }
    public void CloseCheatMenu()
    {
        Debug.LogWarning("Close Cheat Menu Clicked.");
        if (cheatMenuUI != null)
        {
            cheatMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            MusicManager.Instance?.ResumeMusic();
        }
        else
        {
            Debug.LogWarning("Cheat menu UI is still null.");
        }
    }
    public void QuitGame()
    {
        Debug.LogWarning("Quit Clicked.");
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager Instance;

    public TMPro.TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = true;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;


        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";

    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public float GetTime()
    {
        return elapsedTime;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (timerText == null)
        {
            GameObject timerGO = GameObject.FindWithTag("TimerText");
            if (timerGO != null)
            {
                timerText = timerGO.GetComponent<TMPro.TextMeshProUGUI>();
            }
        }

        //ResetTimer();
        StartTimer();
    }
}

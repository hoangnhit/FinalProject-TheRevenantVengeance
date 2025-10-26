using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float countdownTime = 180f;
    private bool bossSpawned = false;

    private PlayerController player;
    public GameObject bossEnemy;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private GameObject introduction;
    [SerializeField] private GameObject dead;
    [SerializeField] private GameObject uiComponent;

    [SerializeField] private AudioClip deathClip;
    [SerializeField] private TMP_Text gateMessageText;
    void Start()
    {
        Time.timeScale = 0f;
        introduction.SetActive(true);
        uiComponent.SetActive(false);

        UpdateTimerUI();
        bossEnemy.SetActive(false);
    }

    void Update()
    {
        if (!bossSpawned)
        {
            countdownTime -= Time.deltaTime;

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                SpawnBoss();
            }

            UpdateTimerUI();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        introduction.SetActive(false);
        uiComponent.SetActive(true);
    }

    public void Dead()
    {
        MuteAllAudioSources();

        // Dynamically create and play death sound
        if (deathClip != null)
        {
            AudioSource deathAudioSource = gameObject.AddComponent<AudioSource>();
            deathAudioSource.clip = deathClip;
            deathAudioSource.Play();

            // Optionally destroy the AudioSource after the clip finishes
            Destroy(deathAudioSource, deathClip.length);
        }

        Time.timeScale = 0f;
        dead.SetActive(true);
    }

    private void MuteAllAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in allAudioSources)
        {
            src.mute = true;
        }
    }

    private void UnmuteAllAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in allAudioSources)
        {
            src.mute = false;
        }
    }

    public void PlayAgain()
    {
        UnmuteAllAudioSources();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("StartMenuScene");
    }

    public void UpdateHealthBarUI(float currentHp, float maxHp)
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
            healthText.text = $"{currentHp} / {maxHp}";
        }
    }

    public void UpdateEnergyBarUI(float currentEnergy, float maxEnergy)
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
        }
    }

    public void UpdateExpBarUI(float currentExp, float maxExp)
    {
        if (expBar != null)
        {
            expBar.fillAmount = currentExp / maxExp;
        }
    }

    public void UpdateLevelUI(int level)
    {
        levelText.text = $"{level}";
    }

    void UpdateTimerUI()
    {
        if (!bossSpawned)
        {
            int minutes = Mathf.FloorToInt(countdownTime / 60f);
            int seconds = Mathf.FloorToInt(countdownTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        bossEnemy.SetActive(true);
        timerText.text = "The Overlord Descends";
        timerText.fontSize = 72;
        timerText.color = Color.red;
    }

    public void OnBossDead()
    {
        if (timerText != null)
        {
            timerText.text = "";
        }

        if (gateMessageText != null)
        {
            gateMessageText.text = "The Gate is Open!";
        }
    }
}
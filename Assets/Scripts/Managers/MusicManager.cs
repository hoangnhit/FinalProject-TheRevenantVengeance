using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ?? bi?t khi nào Scene thay ??i

public class MusicManager : MonoBehaviour
{
    //public static MusicManager instance; // Singleton pattern ?? ??m b?o ch? có 1 MusicManager

    //[SerializeField] private AudioClip backgroundMusic;
    //private AudioSource audioSource;

    //void Awake()
    //{
    //	// Th?c hi?n Singleton pattern
    //	if (instance == null)
    //	{
    //		instance = this;
    //		DontDestroyOnLoad(gameObject); // R?t quan tr?ng: Gi? GameObject này t?n t?i qua các Scene
    //		audioSource = GetComponent<AudioSource>();

    //		if (audioSource == null)
    //		{
    //			audioSource = gameObject.AddComponent<AudioSource>();
    //		}

    //		// Thi?t l?p AudioSource cho nh?c n?n
    //		audioSource.clip = backgroundMusic;
    //		audioSource.loop = true; // L?p l?i liên t?c
    //		audioSource.playOnAwake = false; // Chúng ta s? t? ?i?u khi?n vi?c phát

    //		// ??ng ký s? ki?n khi Scene ???c load
    //		SceneManager.sceneLoaded += OnSceneLoaded;
    //	}
    //	else
    //	{
    //		// N?u ?ã có m?t MusicManager khác, h?y b? cái m?i này
    //		Destroy(gameObject);
    //	}
    //}

    //void OnDestroy()
    //{
    //	// H?y ??ng ký s? ki?n khi GameObject này b? h?y
    //	SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //	// Khi m?t Scene m?i ???c load
    //	// B?n có th? thêm logic ? ?ây ?? ki?m tra Scene nào nên phát nh?c
    //	// ho?c d?ng nh?c n?u ?ó là Scene menu, v.v.

    //	// Ví d?: Phát nh?c n?u ch?a phát
    //	if (!audioSource.isPlaying)
    //	{
    //		audioSource.Play();
    //	}

    //	// Ví d?: D?ng nh?c n?u vào Scene "MainMenu"
    //	// if (scene.name == "MainMenu")
    //	// {
    //	//     audioSource.Stop();
    //	// }
    //	// else if (!audioSource.isPlaying) // Phát n?u không ph?i MainMenu và ch?a phát
    //	// {
    //	//     audioSource.Play();
    //	// }
    //}

    //// Các hàm công c?ng ?? ?i?u khi?n nh?c t? các script khác
    //public void PlayMusic()
    //{
    //	if (audioSource != null && !audioSource.isPlaying)
    //	{
    //		audioSource.Play();
    //	}
    //}

    //public void StopMusic()
    //{
    //	if (audioSource != null && audioSource.isPlaying)
    //	{
    //		audioSource.Stop();
    //	}
    //}

    //public void SetVolume(float volume)
    //{
    //	if (audioSource != null)
    //	{
    //		audioSource.volume = volume;
    //	}
    //}

    public static MusicManager Instance;

    private AudioSource audioSource;

    // Cấu hình ánh xạ tên scene → AudioClip
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip Lv11Music;
    [SerializeField] private AudioClip Lv12Music;
    [SerializeField] private AudioClip Lv13Music;
    [SerializeField] private AudioClip Lv2Music;
    [SerializeField] private AudioClip Lv22Music;
    [SerializeField] private AudioClip Lv23Music;
    [SerializeField] private AudioClip Lv31Music;
    [SerializeField] private AudioClip Lv32Music;
    [SerializeField] private AudioClip Lv33Music;

    private Dictionary<string, AudioClip> sceneMusicMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MusicManager initialized and preserved");

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.loop = true;
            audioSource.playOnAwake = false;

            // Tạo mapping giữa scene và nhạc
            sceneMusicMap = new Dictionary<string, AudioClip>
            {
                { "SceneLv1.1", Lv11Music },
                { "SceneLv1.2", Lv12Music },
                { "SceneLv1.3", Lv13Music },
                { "SceneLv2", Lv2Music },
                { "SceneLv2-2", Lv22Music },
                { "SceneLv2-3", Lv23Music },
                { "SceneLv3.1", Lv31Music },
                { "SceneLv3.2", Lv32Music },
                { "SceneLv3.3", Lv33Music }
                // Add thêm tại đây nếu có nhiều scene hơn
            };

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.Log("Duplicate MusicManager destroyed");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        AudioClip newClip = defaultMusic;

        if (sceneMusicMap.ContainsKey(sceneName))
        {
            newClip = sceneMusicMap[sceneName];
        }

        // Nếu clip mới khác clip hiện tại thì đổi nhạc
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

    // Hàm công cộng nếu muốn đổi nhạc từ nơi khác
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null && audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
    public void PauseMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.UnPause();
    }

    public void RestartMusic()
    {
        audioSource.Stop();
        audioSource.time = 0f;
        audioSource.Play();
    }
}
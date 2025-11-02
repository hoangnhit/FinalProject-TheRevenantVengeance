using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneTransitionManager initialized.");
        }
        else
        {
            Debug.Log("Duplicate SceneTransitionManager detected. Destroying.");
            Destroy(gameObject);
        }
    }

    public void TransitionToScene(string sceneName)
    {
        Debug.Log($"Starting transition to scene: {sceneName}");
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        Debug.Log("Beginning fade out...");
        yield return StartCoroutine(FadeOut());

        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);

        yield return null; // wait 1 frame

        // Reassign fadeCanvasGroup if needed (in case it gets lost between scenes)
        if (fadeCanvasGroup == null)
        {
            fadeCanvasGroup = FindFirstObjectByType<CanvasGroup>();
            if (fadeCanvasGroup != null)
                Debug.Log("fadeCanvasGroup reassigned successfully.");
            else
                Debug.LogWarning("fadeCanvasGroup is still null after scene load.");
        }

        Debug.Log("Beginning fade in...");
        yield return StartCoroutine(FadeIn());

        Debug.Log("Scene transition complete.");
    }

    private IEnumerator FadeOut()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;

            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = Mathf.Clamp01(time / fadeDuration);
            }
            else
            {
                Debug.LogWarning("fadeCanvasGroup is null during FadeOut!");
                yield break;
            }

            yield return null;
        }

        Debug.Log("Fade out finished.");
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;

            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(time / fadeDuration);
            }
            else
            {
                Debug.LogWarning("fadeCanvasGroup is null during FadeIn!");
                yield break;
            }

            yield return null;
        }

        Debug.Log("Fade in finished.");
    }
}

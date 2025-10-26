using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonController : MonoBehaviour
{
    private Vector3 originalScale;
    private AudioSource audioSource;

    [Header("Hover Sound")]
    public AudioClip hoverSound;

    private void Start()
    {
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    public void ScaleUp(BaseEventData data)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
        transform.localScale = originalScale * 1.2f;

        
    }

    public void ScaleDown(BaseEventData data)
    {
        transform.localScale = originalScale;
    }
}

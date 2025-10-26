using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TestUlti : MonoBehaviour
{
    public Canvas ultimateCanvas;
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (ultimateCanvas != null) ultimateCanvas.enabled = true;
        if (videoPlayer != null) videoPlayer.Play();

    }
}

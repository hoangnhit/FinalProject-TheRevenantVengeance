using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject introduction;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject aboutPanel;


    void Start()
    {
        startMenu.SetActive(true);
        introduction.SetActive(false);
        instructionPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    private void Update()
    {

    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        introduction.SetActive(true);
    }

    public void ShowInstructions()
    {
        instructionPanel.SetActive(true);
    }
    public void HideInstructions()
    {
        instructionPanel.SetActive(false);
    }
    public void ShowAbout()
    {
        aboutPanel.SetActive(true);
    }
    public void HideAbout()
    {
        aboutPanel.SetActive(false);
    }
    public void SkipIntro()
    {
        SceneManager.LoadScene("SceneLv1.1");
    }
    public void OnQuitButtonClicked()
    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
    Application.Quit();
//#endif
    }
}

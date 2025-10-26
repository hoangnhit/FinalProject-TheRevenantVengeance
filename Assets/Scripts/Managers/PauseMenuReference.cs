using UnityEngine;

public class PauseMenuReference : MonoBehaviour
{
    void Start()
    {
        Debug.Log("PauseMenuReference Awake called");
        if (GameStateManager.instance != null)
        {
            Debug.Log("Assigning pauseMenuUI from PauseMenuReference");
            GameStateManager.instance.pauseMenuUI = gameObject;
            gameObject.SetActive(false);
        }
    }
}

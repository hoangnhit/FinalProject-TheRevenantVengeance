using UnityEngine;

public class CheatMenuReference : MonoBehaviour
{
    void Start()
    {
        Debug.Log("CheatMenuReference Awake called");
        if (GameStateManager.instance != null)
        {
            Debug.Log("Assigning pauseMenuUI from PauseMenuReference");
            GameStateManager.instance.cheatMenuUI = gameObject;
            gameObject.SetActive(false);
        }
    }
}

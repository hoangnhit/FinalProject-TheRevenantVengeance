using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public float speed = 50f; 
    public float stopYPosition = 1000f; 
    private RectTransform rectTransform;
    private bool isMoving = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isMoving)
        {
            rectTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;

            if (rectTransform.anchoredPosition.y >= stopYPosition)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, stopYPosition);
                isMoving = false; 
            }
        }
    }
}

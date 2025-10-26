using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public float floatUpSpeed = 1f;
    public float disappearTime = 1f;

    private void Start()
    {
        Destroy(gameObject, disappearTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * floatUpSpeed * Time.deltaTime);
    }

    public void SetDamage(float damage)
    {
        textMesh.text = damage.ToString("F0");
    }
}

using UnityEngine;

public class AuraRotator : MonoBehaviour
{
    public float rotationSpeed = 100f;  // T?c ?? quay

    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

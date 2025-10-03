using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

        transform.position = smoothPos;
    }
}
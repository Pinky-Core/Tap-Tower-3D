using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // A quién seguir (el bloque más reciente)
    public float followSpeed = 2f;
    public float yOffset = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(0, target.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}

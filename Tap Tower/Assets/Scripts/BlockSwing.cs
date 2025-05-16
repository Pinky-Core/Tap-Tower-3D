using UnityEngine;

public class BlockSwing : MonoBehaviour
{
    public float swingAmplitude = 3f; // Qué tan lejos se mueve a los lados
    public float swingSpeed = 2f;     // Qué tan rápido se mueve

    private Vector3 startPosition;
    private float time;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime * swingSpeed;
        transform.position = startPosition + Vector3.right * Mathf.Sin(time) * swingAmplitude;
    }
}

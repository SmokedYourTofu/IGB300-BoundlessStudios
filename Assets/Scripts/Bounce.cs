using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float amplitude = 0.5f; // The height of the bounce
    public float frequency = 1f; // The speed of the bounce

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position = initialPosition + new Vector3(0, Mathf.Sin(Time.time * frequency) * amplitude, 0);
    }
}

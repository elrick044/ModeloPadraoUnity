using UnityEngine;

public class QuarkCoreController : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeedY = 25f;
    public float rotationSpeedX = 15f;

    [Header("Pulse Animation")]
    public float pulseSpeed = 2f;
    public float pulseMagnitude = 0.05f;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // --- Rotação ---
        transform.Rotate(Vector3.up, rotationSpeedY * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, rotationSpeedX * Time.deltaTime, Space.World);

        // --- Pulsação (Pulse) ---
        float pulseFactor = 1.0f + (Mathf.Sin(Time.time * pulseSpeed) * pulseMagnitude);
        

        
        transform.localScale = initialScale * pulseFactor;
    }
}
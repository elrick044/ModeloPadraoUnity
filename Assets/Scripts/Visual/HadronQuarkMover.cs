using UnityEngine;

public class HadronQuarkMover : MonoBehaviour
{
    [Header("Configurações da Órbita")]
    public float orbitalSpeed = 30f;
    public float radius = 0.4f;
    [Range(0f, 1f)]
    public float radiusVariation = 0.2f;

    [Header("Configurações da Vibração (Jitter)")]
    public float jitterAmount = 0.01f;
    public float axisWobbleSpeed = 0.5f;

    // Variáveis privadas para controlar a órbita
    private Vector3 initialLocalPosition;
    private Vector3 orbitalAxis;
    private float currentAngle = 0f;
    private float noiseOffset;

    void Start()
    {
        if (transform.parent != null)
        {
            // Começa em uma posição aleatória na esfera do raio definido
            transform.localPosition = Random.onUnitSphere * radius;
            initialLocalPosition = transform.localPosition;
        }
        else
        {
            Debug.LogError("HadronQuarkMover precisa estar em um objeto filho!");
            this.enabled = false;
            return;
        }
        
        noiseOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Calcular a órbita em Espaço Local
        currentAngle += orbitalSpeed * Time.deltaTime;

        Vector3 orbitalPosition = new Vector3(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius,
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius,
            0 
        );

        //Adicionar o "Caos"

        float noiseX = Mathf.PerlinNoise(Time.time * axisWobbleSpeed, noiseOffset + 0.1f) * 2f - 1f;
        float noiseY = Mathf.PerlinNoise(Time.time * axisWobbleSpeed, noiseOffset + 0.2f) * 2f - 1f;
        float noiseZ = Mathf.PerlinNoise(Time.time * axisWobbleSpeed, noiseOffset + 0.3f) * 2f - 1f;
        orbitalAxis = new Vector3(noiseX, noiseY, noiseZ).normalized;
        

        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, orbitalAxis);
        Vector3 finalPosition = rotation * orbitalPosition;

        // Varia o raio da órbita
        float radiusNoise = Mathf.PerlinNoise(Time.time * axisWobbleSpeed, noiseOffset + 0.4f);
        float targetRadius = radius + (radiusNoise * radiusVariation) - (radiusVariation / 2f);
        finalPosition = finalPosition.normalized * targetRadius;

        //Adicionar a Vibração
        Vector3 jitter = Random.insideUnitSphere * jitterAmount;

        // Define a posição LOCAL final do objeto
        transform.localPosition = finalPosition + jitter;
    }
}
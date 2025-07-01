using UnityEngine;

public class WaveAuraController : MonoBehaviour
{
    [Header("Wave Animation")]
    public float waveSpeed = 1.5f;
    public float minScale = 1.2f;
    public float maxScale = 1.5f;
    public float minAlpha = 0.1f;
    public float maxAlpha = 0.5f;

    private Transform cameraTransform;
    private Material material;
    private Vector3 initialScale;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        
        material = GetComponent<Renderer>().material;

        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.LookAt(cameraTransform);

        // --- Animação de Onda (Wave Expand) ---
        // Usa Mathf.PingPong para criar um efeito de vai e vem suave entre 0 e 1.
        float t = Mathf.PingPong(Time.time * waveSpeed, 1f);

        // Interpola a escala e o alfa (transparência)
        float currentScale = Mathf.Lerp(minScale, maxScale, t);
        float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
        
        transform.localScale = initialScale * currentScale;
        
        Color currentColor = material.color;
        currentColor.a = currentAlpha;
        material.color = currentColor;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ParticleUIController : MonoBehaviour
{

    public Image moldura;
    public TMPro.TextMeshPro symbolText;
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI energiaText;
    public TMPro.TextMeshProUGUI massaText;
    public TMPro.TextMeshProUGUI spinText;
    public GameObject core;

    public void SetData(ParticleData data)
{
    Transform frame = transform.Find("InforCard-Frame");
    if (frame == null)
    {
        Debug.LogError("InforCard-Frame não encontrado!");
        return;
    }

    Transform oldCore = frame.Find("Core");
    
    if (oldCore != null)
    {
    #if UNITY_EDITOR
        DestroyImmediate(oldCore.gameObject);
    #else
        Destroy(oldCore.gameObject);
    #endif
    }

    if (data.model != null)
    {
        GameObject newCore = Instantiate(data.model, frame);
        newCore.name = "Core";
        newCore.transform.localPosition = Vector3.zero;
        newCore.transform.localRotation = Quaternion.identity;
        newCore.transform.localScale = Vector3.one * 30.0f;

        TMPro.TextMeshPro symbolTextInCore = newCore.GetComponentInChildren<TMPro.TextMeshPro>();

        if (symbolTextInCore != null)
        {
            symbolTextInCore.text = data.symbol;
        }
        else
        {
            Debug.LogWarning("Nenhum objeto 'SymbolText' com componente TextMeshPro foi encontrado dentro do novo Core instanciado.");
        }
    }

    nameText.text = data.particleName;
    energiaText.text = data.energy;
    massaText.text = data.mass;
    spinText.text = data.spin.ToString();

    nameText.color = data.color;
    energiaText.color = data.color;
    massaText.color = data.color;
    spinText.color = data.color;

}
}

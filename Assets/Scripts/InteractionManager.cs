using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    private List<ParticleMarkerBehaviour> visibleParticles = new List<ParticleMarkerBehaviour>();
    private HashSet<ParticleMarkerBehaviour> usedParticles = new HashSet<ParticleMarkerBehaviour>();


    [Header("Prefabs e Dados")]
    public GameObject infoCardPrefab;
    public ParticleData gluonData;
    public ParticleData protonData;
    public ParticleData neutronData;

    [Header("Configurações")]
    public float gluonTravelTime = 1.0f;
    public float gluonCooldown = 3.0f;

    private Coroutine gluonCoroutine;
    private float lastGluonTime = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


void Update()
{
    if (visibleParticles.Count == 2 && Time.time - lastGluonTime >= gluonCooldown)
    {
        var a = visibleParticles[0];
        var b = visibleParticles[1];

        if (a.particleData.particleType == ParticleType.Quark && b.particleData.particleType == ParticleType.Quark)
        {
            if (a.gameObject.activeInHierarchy && b.gameObject.activeInHierarchy)
            {
                var coreA = FindCore(a.transform);
                var coreB = FindCore(b.transform);

                if (coreA != null && coreB != null)
                {
                    Debug.Log($"Iniciando troca de glúon entre Quarks: {a.particleData.particleName} e {b.particleData.particleName}");
                    StartCoroutine(AnimateGluon(coreA.position, coreB.position));
                    lastGluonTime = Time.time;
                }
            }
        }
    }
    if (gluonCoroutine == null)
    {
        CheckTriadInteractions();
    }
}

    public void RegisterVisibleParticle(ParticleMarkerBehaviour p)
    {
        if (!visibleParticles.Contains(p))
        {
            visibleParticles.Add(p);
            Debug.Log("Nova partícula visível: " + p.particleData.symbol);
        }
    }

    public void UnregisterVisibleParticle(ParticleMarkerBehaviour p)
    {
        if (visibleParticles.Contains(p))
            visibleParticles.Remove(p);
    }

    Transform FindCore(Transform root)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "Core")
                return child;
        }
        return null;
    }

    void CheckTriadInteractions()
    {
        List<ParticleMarkerBehaviour> ups = new List<ParticleMarkerBehaviour>();
        List<ParticleMarkerBehaviour> downs = new List<ParticleMarkerBehaviour>();

        foreach (var p in visibleParticles)
        {
            if (usedParticles.Contains(p)) continue; // Ignora os já usados

            if (p.particleData.symbol == "U") ups.Add(p);
            else if (p.particleData.symbol == "D") downs.Add(p);
        }

        // PRÓTON: U U D
        if (ups.Count >= 2 && downs.Count >= 1)
        {
            var up1 = ups[0];
            var up2 = ups[1];
            var down = downs[0];

            if (AllClose(up1.transform, up2.transform, down.transform, 0.5f))
            {
                var c1 = FindCore(up1.transform);
                var c2 = FindCore(up2.transform);
                var c3 = FindCore(down.transform);

                if (c1 && c2 && c3)
                {
                    Debug.Log("Próton formado!");
                    usedParticles.Add(up1);
                    usedParticles.Add(up2);
                    usedParticles.Add(down);
                    gluonCoroutine = StartCoroutine(AnimateGluonCycle(c1, c2, c3, protonData));
                    return;
                }
            }
        }

        // NÊUTRON: D D U
        if (downs.Count >= 2 && ups.Count >= 1)
        {
            var down1 = downs[0];
            var down2 = downs[1];
            var up = ups[0];

            if (AllClose(down1.transform, down2.transform, up.transform, 0.5f))
            {
                var c1 = FindCore(down1.transform);
                var c2 = FindCore(down2.transform);
                var c3 = FindCore(up.transform);

                if (c1 && c2 && c3)
                {
                    Debug.Log("Nêutron formado!");
                    usedParticles.Add(down1);
                    usedParticles.Add(down2);
                    usedParticles.Add(up);
                    gluonCoroutine = StartCoroutine(AnimateGluonCycle(c1, c2, c3, neutronData));
                    return;
                }
            }
        }
    }


    bool AllClose(Transform a, Transform b, Transform c, float maxDistance)
    {
        return Vector3.Distance(a.position, b.position) <= maxDistance &&
               Vector3.Distance(a.position, c.position) <= maxDistance &&
               Vector3.Distance(b.position, c.position) <= maxDistance;
    }

    IEnumerator AnimateGluon(Vector3 start, Vector3 end)
    {
        GameObject g = Instantiate(infoCardPrefab, start, Quaternion.identity);
        g.transform.localScale = Vector3.one * 0.15f;

        var ui = g.GetComponent<ParticleUIController>();
        if (ui != null)
            ui.SetData(gluonData);

        float elapsed = 0f;

        while (elapsed < gluonTravelTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / gluonTravelTime;
            g.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        Destroy(g);
    }

    IEnumerator AnimateGluonCycle(Transform c1, Transform c2, Transform c3, ParticleData compositeData)
    {
        GameObject g = Instantiate(infoCardPrefab);
        g.transform.localScale = Vector3.one * 0.15f;

        var ui = g.GetComponent<ParticleUIController>();
        if (ui != null)
            ui.SetData(gluonData);

        float t = 0f;
        while (t < 3f)
        {
            float time = Mathf.Repeat(t, 3f);
        if (c1 == null || c2 == null || c3 == null)
            break;

        if (time < 1f)
            g.transform.position = Vector3.Lerp(c1.position, c2.position, time);
        else if (time < 2f)
            g.transform.position = Vector3.Lerp(c2.position, c3.position, time - 1f);
        else
            g.transform.position = Vector3.Lerp(c3.position, c1.position, time - 2f);


            t += Time.deltaTime * (1 / gluonTravelTime);
            yield return null;
        }

        Destroy(g);

        Vector3 center = (c1.position + c2.position + c3.position) / 3f;

        GameObject comp = Instantiate(infoCardPrefab, center, Quaternion.identity);
        comp.transform.localScale = Vector3.one * 0.37f;

        var compUI = comp.GetComponent<ParticleUIController>();
        if (compUI != null)
            compUI.SetData(compositeData);


        c1.parent.gameObject.SetActive(false);
        c2.parent.gameObject.SetActive(false);
        c3.parent.gameObject.SetActive(false);

        gluonCoroutine = null;
    }
}

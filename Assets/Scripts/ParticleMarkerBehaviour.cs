using UnityEngine;
using Vuforia;

public class ParticleMarkerBehaviour : MonoBehaviour
{
    public GameObject infoCardPrefab;
    public ParticleData particleData;

    private ObserverBehaviour observer;
    private GameObject instantiatedCard;

    //public Camera camera;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if (instantiatedCard == null)
            {
                instantiatedCard = Instantiate(infoCardPrefab, transform);
                instantiatedCard.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                instantiatedCard.transform.localPosition = new Vector3(0f, 0.02f, 0f);


                var ui = instantiatedCard.GetComponent<ParticleUIController>();
                
                ui?.SetData(particleData);
            }

            InteractionManager.Instance.RegisterVisibleParticle(this);
        }
        else
        {
            if (instantiatedCard != null)
            {
                Destroy(instantiatedCard);
            }

            InteractionManager.Instance.UnregisterVisibleParticle(this);
        }
    }
}

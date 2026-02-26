using System.Collections;
using UnityEngine;

public class CupAnomalyTwo : AnomalyReciever
{


    [SerializeField]
    private BoxCollider handEnterZone;

    [SerializeField]
    private MeshRenderer cupRenderer;

    [SerializeField]
    private MeshCollider cupCollider;

    [SerializeField]
    private Rigidbody cupRigidbody;

    [SerializeField]
    private bool VerboseLogging = false;

    //Make this into a list of consistent times if you want the flicker duration to differ each attempt from the participant
    [SerializeField]
    private float flickerDuration = 1;

    private Coroutine flickerCupCoroutine;

    protected override void SetupObjectAtStart()
    {
        base.SetupObjectAtStart();

        handEnterZone.enabled = false;


        if (VerboseLogging)
        {
            Debug.Log("SetupObjectAtStart()");

        }
    }


    protected override void AnomalyEnabled()
    {
        if (VerboseLogging)
        {
            Debug.Log("AnomalyEnabled");
        }

        handEnterZone.enabled = true;
    }

    protected override void AnomalyDisabled()
    {
        handEnterZone.enabled = false;

        if (VerboseLogging)
        {
            Debug.Log("AnomalyDisabled");
        }
    }

    protected override void CancelAnomaly()
    {
        if (flickerCupCoroutine != null)
        {
            StopCoroutine(flickerCupCoroutine);
        }

        returnCupToStandard();
    }

    //Will only happen when the collider is enabled (which is only when the anomaly is enabled)

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            if (VerboseLogging)
            {
                Debug.Log("Hand entered zone");
            }

            //Flicker cup
            if (flickerCupCoroutine == null)
            {
                flickerCupCoroutine = StartCoroutine(flickerCup());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Hand")
        {
            if (VerboseLogging)
            {
                Debug.Log("Hand exited zone");
            }

            
        }




    }


    private IEnumerator flickerCup()
    {
        handEnterZone.enabled = false;
        cupRigidbody.isKinematic = true;
        cupCollider.isTrigger = true;
        cupRenderer.enabled = false;

        yield return new WaitForSeconds(flickerDuration);

        returnCupToStandard();
    }

    private void returnCupToStandard()
    {
        cupRigidbody.isKinematic = false;
        cupCollider.isTrigger = false;
        cupRenderer.enabled = true;
        handEnterZone.enabled = true;
    }

    




}

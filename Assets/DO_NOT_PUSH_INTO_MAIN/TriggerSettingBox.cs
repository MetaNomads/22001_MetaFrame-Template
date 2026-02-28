using UnityEngine;

public class TriggerSetting : MonoBehaviour
{
    private bool CupPresent;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Cup")) return;

        CupPresent = true;
        Debug.Log($"Cup entered trigger: {other.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Cup")) return;

        CupPresent = false;
        Debug.Log($"Cup exited trigger: {other.name}");
    }

    public bool GetCupPresence()
    { 
        return CupPresent; 
    }
}


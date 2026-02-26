using UnityEngine;

public class TriggerSetting : MonoBehaviour
{
    private bool CupPresent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cup"))
        {
            CupPresent = true;
            Debug.Log("Functions Activation: True");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cup"))
        {
            CupPresent = false;
            Debug.Log("Functions Activation: False");
        }
    }

    public bool GetCupPresence()
    { 
        return CupPresent; 
    }
}


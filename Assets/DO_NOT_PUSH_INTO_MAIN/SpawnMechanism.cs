using MetaFrame.Data;
using UnityEngine;

public class SpawnMechanism : MonoBehaviour
{
    [SerializeField] private GameObject cupPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TriggerSetting PlateTrigger;
    [SerializeField] private TriggerSetting PlateTrigger2;

    private GameObject currentCup;

    public void SpawnCup()
    {
        currentCup = Instantiate(
            cupPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }

    public void DestroyCup()
    {
        if (currentCup != null)
        {
            Destroy(currentCup);
            currentCup = null;
        }
    }
}

using MetaFrame.Data;
using UnityEngine;

public class SpawnMechanism : MonoBehaviour
{
    [SerializeField] private GameObject cupPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform spawnPoint2;
    [SerializeField] private TriggerSetting PlateTrigger;
    [SerializeField] private TriggerSetting PlateTrigger2;


    private GameObject currentCup;
    private string cupPlaced;
    private Transform currentCupSpawnPoint = null;

    public Transform CurrentPlateSpawnPosition()
    {
     return currentCupSpawnPoint;
    }

    public string CupPlacementCompletion()
    { 
        Debug.Log("GSM cup:" + cupPlaced);  
        return cupPlaced; 
    }

    public Transform CurrentPlateSpawn()
    {
        Transform chosenPlate =
                Random.value < 0.5f ? spawnPoint : spawnPoint2;

        currentCupSpawnPoint = chosenPlate;
        return chosenPlate;
    }

    public void SpawnCup()
    {
        Transform chosenPlate = CurrentPlateSpawn();
        currentCup = Instantiate(
            cupPrefab,
            chosenPlate.position,
            chosenPlate.rotation
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

    public string cupPlacement()
    {
        if (CurrentPlateSpawnPosition() == spawnPoint && PlateTrigger2.GetCupPresence() == true)
        {
            return cupPlaced = "True";
        }
        else if (CurrentPlateSpawnPosition() == spawnPoint2 && PlateTrigger.GetCupPresence() == true)
        {
            return cupPlaced = "True";
        }
        else
        {
            return cupPlaced = "False";
        }
    }
}

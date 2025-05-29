using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject batteryPrefab;
    [SerializeField]
    private GameObject[] spawnPointsInCabin;
    [SerializeField]
    private GameObject[] spawnPointsOutside;
    [SerializeField]
    [Range(0f, 1f)]
    private float spawnProbabilityInside = 0.25f;
    [SerializeField]
    [Range(0f, 1f)]
    private float spawnProbabilityOutside = 0.7f;
    
    [SerializeField]
    private FlashlightBattery playerFlashlightBattery;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject spawnPoint in spawnPointsInCabin)
        {
            if (Random.value < spawnProbabilityInside)
            {
                GameObject batteryInstance = Instantiate(batteryPrefab, spawnPoint.transform.position, Quaternion.identity);
                
                Battery batteryScript = batteryInstance.GetComponent<Battery>();
                if (batteryScript != null)
                {
                    batteryScript.flashlightBattery = playerFlashlightBattery;
                }
            }
        }
        
        foreach (GameObject spawnPoint in spawnPointsOutside)
        {
            if (Random.value < spawnProbabilityOutside)
            {
                GameObject batteryInstance = Instantiate(batteryPrefab, spawnPoint.transform.position, Quaternion.identity);
                
                Battery batteryScript = batteryInstance.GetComponent<Battery>();
                if (batteryScript != null)
                {
                    batteryScript.flashlightBattery = playerFlashlightBattery;
                }
            }
        }
    }
}

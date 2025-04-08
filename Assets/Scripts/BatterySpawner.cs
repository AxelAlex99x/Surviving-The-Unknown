using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject batteryPrefab;
    [SerializeField]
    private GameObject[] spawnPoints; 
    [SerializeField]
    [Range(0f, 1f)]
    private float spawnProbability = 0.5f;
    
    [SerializeField]
    private FlashlightBattery playerFlashlightBattery;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        // flashlightBattery.compone
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (Random.value < spawnProbability)
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

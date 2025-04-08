using UnityEngine;

public class Battery : Interactable
{
    [SerializeField]
    public FlashlightBattery flashlightBattery;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        if(flashlightBattery.currentBattery < flashlightBattery.maxBattery)
        {
            flashlightBattery.RechargeBattery(10);
            flashlightBattery.UpdateBatteryUI();
            Object.Destroy(this.gameObject);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    public float maxBattery = 100f;
    public float currentBattery = 0f;
    
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private Slider batterySlider;
    [SerializeField] private float drainRate = 1.5f;
    [SerializeField] private TMP_Text batteryTextTMP;
    private bool hasBattery = true;
    
    public bool HasBattery => currentBattery > 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBattery = maxBattery;
        UpdateBatteryUI();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (flashlight.IsActive && hasBattery)
        {
            DrainBattery();
            UpdateBatteryUI();
            // CheckLowBattery();
        }
    }
    
    private void DrainBattery()
    {
        currentBattery -= drainRate * Time.deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        if (currentBattery <= 0f)
        {
            hasBattery = false;
            flashlight.TurnOff();
        }
    }
    
    public void RechargeBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }
    
    public void UpdateBatteryUI()
    {
        if (batterySlider != null)
        {
            // batterySlider.value = currentBattery / maxBattery;
            batterySlider.value = currentBattery;
            // batteryTextTMP.text = currentBattery.ToString();
            if (currentBattery <= 20f)
            {
                batteryTextTMP.color = Color.red;
            } else batteryTextTMP.color = Color.white;
            batteryTextTMP.text = $"{Mathf.RoundToInt(currentBattery)}%";
        }
    }
}

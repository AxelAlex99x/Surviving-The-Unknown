using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorFuel : MonoBehaviour
{
    [SerializeField] private Fuel fuelCan;
    
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 5f;
    public Slider fuelSlider;
    public TextMeshProUGUI fuelText;
    public Slider refuelProgressSlider;
    public float refuelDuration = 7f;
    public LayerMask generatorLayer;

    private Generator generator;
    public float currentFuel;
    public bool isRefueling = false;
    private float refuelTimer = 0f;
    private Camera playerCamera;

    void Start()
    {
        generator = GetComponent<Generator>();
        playerCamera = Camera.main;
        currentFuel = 25f;
        fuelSlider.value = currentFuel;
        UpdateFuelUI();
        fuelSlider.gameObject.SetActive(false);
        fuelText.gameObject.SetActive(false);
        refuelProgressSlider.gameObject.SetActive(false);
        Debug.Log("GeneratorFuel script started. Refuel duration: " + refuelDuration);
        Debug.Log("Refuel slider max value: " + refuelProgressSlider.maxValue);
    }

    void Update()
    {
        CheckForLookRay();
        
        if (generator.currentState == Generator.GeneratorState.Running)
        {
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(currentFuel, 0f);
            UpdateFuelUI();

            if (currentFuel <= 0f)
            {
                generator.StopGenerator();
            }
        }

        if (isRefueling)
        {
            refuelTimer += Time.deltaTime;
            float progress = refuelTimer / refuelDuration;
            refuelProgressSlider.value = progress;
            //refuelProgressSlider.value = refuelTimer;
            Debug.Log($"Refuel Timer: {refuelTimer:F2}, Progress :{progress:F2}, Slider Value: {refuelProgressSlider.value:F2}");
            if (refuelTimer >= refuelDuration)
            {
                currentFuel = Mathf.Min(currentFuel + 50f, maxFuel);
                UpdateFuelUI();
                CancelRefuel();
                generator.ConsumeFuelCan();
            }
        }
    }

    void CheckForLookRay()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f, generatorLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                fuelSlider.gameObject.SetActive(true);
                fuelText.gameObject.SetActive(true);
                return;
            }
        }
        
        fuelSlider.gameObject.SetActive(false);
        fuelText.gameObject.SetActive(false);
    }

    public void StartRefuel()
    {
        if (!isRefueling && currentFuel < maxFuel)
        {
            isRefueling = true;
            refuelTimer = 0f;
            refuelProgressSlider.gameObject.SetActive(true);
            refuelProgressSlider.value = 0f;
        }
    }

    public void CancelRefuel()
    {
        isRefueling = false;
        refuelProgressSlider.gameObject.SetActive(false);
        refuelTimer = 0f;
    }

    private void UpdateFuelUI()
    {
        fuelSlider.value = currentFuel;
        fuelText.text = $"{Mathf.RoundToInt((currentFuel / maxFuel) * 100)}%";
    }
}
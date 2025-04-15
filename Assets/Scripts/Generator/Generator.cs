using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Generator : Interactable
{
    [Header("Settings")]
    [SerializeField] private float minRunTime = 10f;
    [SerializeField] private float maxRunTime = 20f;
    [SerializeField] private float restartDuration = 5f;

    [Header("UI")]
    [SerializeField] private GeneratorFuel fuelSystem;
    [SerializeField] private Slider progressSlider; // Reference to the Slider component
    [SerializeField] private Fuel fuelCan;

    [Header("Fuel")]
    public bool PlayerHasFuelCan { get; set; }
    public enum GeneratorState { Running, Stopped, Restarting }
    public GeneratorState currentState;
    private float timer;
    private float currentRunDuration;
    private AudioSource audioSource;
    [SerializeField] 
    private Light[] spotlights;
    [SerializeField] 
    private GameObject[] lamps;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartGenerator();
        UpdateUI(false);
    }

    void Update()
    {
        switch (currentState)
        {
            case GeneratorState.Running:
                HandleRunningState();
                break;
            
            case GeneratorState.Restarting:
                HandleRestartingState();
                break;
        }
    }

    private void StartGenerator()
    {
        currentState = GeneratorState.Running;
        audioSource.Play();
        foreach (Light light in spotlights)
        {
            light.enabled = true;
        }

        foreach (GameObject lamp in lamps)
        {
            lamp.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 5f);
        }
        currentRunDuration = Random.Range(minRunTime, maxRunTime);
        timer = currentRunDuration;
    }

    private void HandleRunningState()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            currentState = GeneratorState.Stopped;
            audioSource.Stop();
            foreach (Light light in spotlights)
            {
                light.enabled = false;
            }

            foreach (GameObject lamp in lamps)
            {
                lamp.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * -3f);
            }
            Debug.Log("Generator stopped!");
        }
    }

    private void HandleRestartingState()
    {
        timer += Time.deltaTime;
        progressSlider.value = timer / restartDuration;

        if (timer >= restartDuration)
        {
            FinishRestart();
        }
    }

    protected override void HoldInteraction()
    {
        if (currentState == GeneratorState.Stopped)
        {
            
            if (PlayerHasFuelCan && fuelSystem.currentFuel < fuelSystem.maxFuel)
            {
                fuelSystem.StartRefuel();
            }
           
            else if (fuelSystem.currentFuel > 0)
            {
                if (currentState != GeneratorState.Restarting)
                {
                    currentState = GeneratorState.Restarting;
                    timer = 0f;
                    UpdateUI(true);
                }

                timer += Time.deltaTime;
                progressSlider.value = timer / restartDuration;

                if (timer >= restartDuration)
                    FinishRestart();
            }
        }
        else if (currentState == GeneratorState.Restarting)
        {
            // Continue the restart process
            timer += Time.deltaTime;
            progressSlider.value = timer / restartDuration;

            if (timer >= restartDuration)
                FinishRestart();
        }
    }

    protected override void ReleaseInteraction()
    {
        if (currentState == GeneratorState.Restarting)
        {
            currentState = GeneratorState.Stopped;
            UpdateUI(false);
            timer = 0f;
        }
        else if (fuelSystem.isRefueling)
        {
            fuelSystem.CancelRefuel();
        }
    }

    public void ConsumeFuelCan()
    {
        PlayerHasFuelCan = false; // Reset ownership after refuel
    }
    private void StartRestart()
    {
        currentState = GeneratorState.Restarting;
        timer = 0;
        UpdateUI(true);
    }

    private void FinishRestart()
    {
        UpdateUI(false);
        StartGenerator();
    }

    private void UpdateUI(bool show)
    {
        // Enable/disable the entire slider GameObject
        progressSlider.gameObject.SetActive(show);
        
        // Optional: Reset slider value when hiding
        if (!show) progressSlider.value = 0;
    }
    public void StopGenerator()
    {
        currentState = GeneratorState.Stopped;
        audioSource.Stop();
        foreach (Light light in spotlights)
        {
            light.enabled = false;
        }

        foreach (GameObject lamp in lamps)
        {
            lamp.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * -3f);
        }
        Debug.Log("Generator stopped!");
    }
    
}
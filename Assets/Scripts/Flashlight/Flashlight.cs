using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField]
    GameObject flashlight;
    private bool flashlightOn;
    private Light flashlightColor;
    [SerializeField] private FlashlightBattery battery;
    [SerializeField] private AudioSource flashlightAudio;
    [SerializeField] private AudioClip activateFlashlightSound;
    public Light lightComponent;
    public bool IsActive => flashlightOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        flashlightColor = flashlight.GetComponent<Light>();
        flashlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.playerActions.ToggleFlashlight.WasPressedThisFrame() && battery.HasBattery)
        {
            if (!flashlightOn)
            {
                if(!flashlightAudio.isPlaying)
                flashlightAudio.PlayOneShot(activateFlashlightSound);
                flashlight.SetActive(true);
                flashlightOn = true;
            }
            else
            {
                if(!flashlightAudio.isPlaying)
                flashlightAudio.PlayOneShot(activateFlashlightSound);
                flashlight.SetActive(false);
                flashlightOn = false;
            }
        }

        if (flashlightOn)
        {
            if (inputManager.playerActions.FlashlightColor.IsInProgress())
            {
                if(!flashlightAudio.isPlaying && inputManager.playerActions.FlashlightColor.WasPressedThisFrame())
                    flashlightAudio.PlayOneShot(activateFlashlightSound);
                flashlightColor.color = Color.magenta;
            }
            else
            {
                if(!flashlightAudio.isPlaying && inputManager.playerActions.FlashlightColor.WasReleasedThisFrame())
                    flashlightAudio.PlayOneShot(activateFlashlightSound);
                flashlightColor.color = Color.white;
            }
        }
    }
    
    public void TurnOff()
    {
        flashlight.SetActive(false);
        flashlightOn = false;
    }

    
}

using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField]
    GameObject flashlight;
    private bool flashlightOn;
    private Light flashlightColor;
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
        if (inputManager.playerActions.ToggleFlashlight.WasPressedThisFrame())
        {
            if (!flashlightOn)
            {
                flashlight.SetActive(true);
                flashlightOn = true;
            }
            else
            {
                flashlight.SetActive(false);
                flashlightOn = false;
            }
        }
        if (flashlightOn && inputManager.playerActions.FlashlightColor.IsInProgress())
        {
            flashlightColor.color = Color.magenta;
        }
        else
        {
            flashlightColor.color = Color.white;
        }
    }
}

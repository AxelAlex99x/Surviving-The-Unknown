using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    
    
    private int pendingResolutionIndex; 
    private float pendingVolume;       
    private int pendingQuality;        
    private bool pendingFullscreen;   
    
    public Button applyButton; 
    public Button backButton;
    
    void Start()
    {
        resolutions = Screen.resolutions;
        
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        pendingResolutionIndex = currentResolutionIndex;
        audioMixer.GetFloat("volume", out pendingVolume);
        pendingQuality = QualitySettings.GetQualityLevel();
        pendingFullscreen = Screen.fullScreen;

        
        applyButton.onClick.AddListener(ApplySettings);
        backButton.onClick.AddListener(CloseSettings);
    }

    public void ApplySettings()
    {
        Resolution resolution = resolutions[pendingResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, pendingFullscreen);
        audioMixer.SetFloat("volume", pendingVolume);
        QualitySettings.SetQualityLevel(pendingQuality);
        Screen.fullScreen = pendingFullscreen;
    }
    
    public void CloseSettings()
    {
        resolutionDropdown.value = pendingResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        gameObject.SetActive(false); 
    }
    
    public void SetResolution(int resolutionIndex)
    {
        pendingResolutionIndex = resolutionIndex;
    }
    
    public void SetVolume(float volume)
    {
        pendingVolume = volume;
    }
    
    public void SetQuality(int quality)
    {
        pendingQuality = quality;
    }
    
    public void SetFullscreen(bool fullscreen)
    {
        pendingFullscreen = fullscreen;
    }
    
    // public void SetResolution(int resolutionIndex)
    // {
    //     Resolution resolution = resolutions[resolutionIndex];
    //     Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    // }
    //
    // public void SetVolume(float volume)
    // {
    //     audioMixer.SetFloat("volume", volume);
    // }
    //
    // public void SetQuality(int quality)
    // {
    //     QualitySettings.SetQualityLevel(quality);
    // }
    //
    // public void SetFullscreen(bool fullscreen)
    // {
    //     Screen.fullScreen = fullscreen;
    // }
}

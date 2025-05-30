using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndMenu : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float hoursAfterInitialTime = 9f; 
    private float endTimeInSeconds;

    [Header("References")]
    [SerializeField] private GameClockController gameClock;
    [SerializeField] private GameObject endGameUI;

    public AudioSource audioSource;
    
    private float initialElapsedTime;
    private float timeInADay;
    private bool gameEnded = false;
    
    void Start()
    {
        endGameUI.SetActive(false);
        initialElapsedTime = gameClock.CurrentTime;
        timeInADay = gameClock.TimeInADay;
        endTimeInSeconds = hoursAfterInitialTime * 3600f;
    }

    void Update()
    {
        float currentTime = gameClock.CurrentTime;
        float timePassed = (currentTime - initialElapsedTime + timeInADay) % timeInADay;

        if (!gameEnded && timePassed >= endTimeInSeconds)
        {
             EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;
        Time.timeScale = 0f;
        endGameUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
        Ambience ambienceScript = FindFirstObjectByType<Ambience>();
        RandomSoundEffects soundEffects = FindFirstObjectByType<RandomSoundEffects>();
        WindSound windSound = FindFirstObjectByType<WindSound>();
        ambienceScript.enabled = false;
        soundEffects.enabled = false;
        windSound.enabled = false;
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in allAudioSources)
        {
            if (source != audioSource)
            {
                source.Stop();
            }
        }
        audioSource.Play();
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

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

using System;
using UnityEngine;
using TMPro;
public class GameClockController : MonoBehaviour
{
    [Header("Clock UI")]
    [SerializeField]
    private TMP_Text clockText;
    private float elapsedTime;
    
    [Header("Time in a night")]
    [SerializeField]
    private float timeInADay = 86400f;
    
    [Header("How Fast Time Pass")] 
    [SerializeField]
    private float timeScale = 2f;
    [SerializeField]
    private float howMuchTime = 1f;

    private DayNightCycle dayNightCycle;
    
    public float CurrentTime => elapsedTime;
    public float TimeInADay => timeInADay;
    private void Awake()
    { 
        dayNightCycle = GetComponent<DayNightCycle>(); 
        elapsedTime = 22 * 3600f;
        SetNightDuration(howMuchTime);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime * timeScale;
        elapsedTime %= timeInADay;
        UpdateClockUI();
        dayNightCycle.UpdateLighting(elapsedTime);
    }
    
    public void SetNightDuration(float realTimeMinutes)
    {
        timeScale = (9 * 3600) / (realTimeMinutes * 60);
    }

    void UpdateClockUI()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime - hours * 3600f) / 60f);
        int seconds = Mathf.FloorToInt((elapsedTime - hours * 3600f) - (minutes * 60f));
        
        string ampm = hours < 12 ? "AM" : "PM";
        hours = hours % 12;

        if (hours == 0)
        {
            hours = 12;
        }
        
        string clockString = string.Format("{0:00}:{1:00}:{2:00} {3}", hours, minutes, seconds, ampm);
        clockText.text = clockString;
    }
}

using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField]
    private Light directionalLight; 

    [Header("Sunrise Settings")]
    [SerializeField]
    private float sunriseStart = 6 * 3600f; 
    [SerializeField]
    private float sunriseEnd = 7 * 3600f;   

    public void UpdateLighting(float elapsedTime)
    {
        float normalizedTime = (elapsedTime - (22 * 3600f)) % 86400f; 
        if (normalizedTime < 0) normalizedTime += 86400f;
        
        float sunRotation = Mathf.Lerp(190f, 375f, normalizedTime / (9 * 3600f));
        directionalLight.transform.rotation = Quaternion.Euler(sunRotation, 0, 0);
        
        if (elapsedTime >= sunriseStart && elapsedTime <= sunriseEnd)
        {
            float lerpValue = Mathf.InverseLerp(sunriseStart, sunriseEnd, elapsedTime);
            directionalLight.intensity = Mathf.Lerp(0f, 1f, lerpValue);
            directionalLight.color = Color.Lerp(Color.blue, Color.white, lerpValue);
        }
    }
}
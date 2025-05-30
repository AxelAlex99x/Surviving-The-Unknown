using UnityEngine;

public class RandomSoundEffects : MonoBehaviour
{
    [Header("Door")]
    public AudioSource doorSource;
    public AudioClip[] doorSounds;
    public Vector2 doorInterval = new Vector2(3f, 7f);
    private float nextDoorTime;

    [Header("Windows")]
    public AudioSource[] windowSources; 
    public AudioClip[] windowSounds;    
    public Vector2 windowInterval = new Vector2(3f, 8f);
    private float nextWindowTime;

    private bool isInCabin;
    void Start()
    {
        nextDoorTime = Time.time + Random.Range(doorInterval.x, doorInterval.y);
        nextWindowTime = Time.time + Random.Range(windowInterval.x, windowInterval.y);
    }

    void Update()
    {
        if (isInCabin)
        {
            if (Time.time > nextDoorTime)
            {
                PlayRandomSound(doorSource, doorSounds);
                nextDoorTime = Time.time + Random.Range(doorInterval.x, doorInterval.y);
            }

            if (Time.time > nextWindowTime)
            {
                AudioSource randomWindow = windowSources[Random.Range(0, windowSources.Length)];
                PlayRandomSound(randomWindow, windowSounds);
                nextWindowTime = Time.time + Random.Range(windowInterval.x, windowInterval.y);
            }
        }
    }

    void PlayRandomSound(AudioSource source, AudioClip[] clips)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clip);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CabinZone"))
        {
            isInCabin = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CabinZone"))
        {
            isInCabin = false;
        }
    }
}

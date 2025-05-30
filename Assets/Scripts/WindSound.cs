using UnityEngine;

public class WindSound : MonoBehaviour
{
    public float minDelay = 5f;
    public float maxDelay = 15f;
    
    [SerializeField]
    public AudioSource audioSource;
    private float nextPlayTime;
    //private bool isPlaying;
     
    //private bool isInCabin;
    
    void Start()
    {
        SetNextPlayTime();
    }

    void Update()
    {
        // if (isInCabin)
        // {
            if (!audioSource.isPlaying && Time.time >= nextPlayTime)
            {
                PlaySound();
                SetNextPlayTime();
            }
        // }
        // else
        // {
        //     StopSound();
        // }
    }

    void PlaySound()
    {
        
       // isPlaying = true;
        audioSource.Play();
    }

    void StopSound()
    {
        //isPlaying = false;
        audioSource.Stop();
    }
    
    void SetNextPlayTime()
    {
        nextPlayTime = Time.time + Random.Range(minDelay, maxDelay);
    }
    
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("CabinZone"))
    //     {
    //         isInCabin = true;
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("CabinZone"))
    //     {
    //         isInCabin = false;
    //     }
    // }
}


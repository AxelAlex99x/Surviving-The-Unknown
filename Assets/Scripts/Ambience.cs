using UnityEngine;

public class Ambience : MonoBehaviour
{
    public Collider area;

    public GameObject player;
    
    [SerializeField]
    private AudioSource ambientAudioSource;

    [SerializeField] 
    private AudioClip normalAmbientSound;
    [SerializeField]
    private AudioClip horrorAmbientSound;

    private bool isInCabin;
    
    [SerializeField]
    Generator generator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ambientAudioSource.clip = normalAmbientSound;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 closestPoint = area.ClosestPoint(player.transform.position);
        // transform.position = closestPoint;

        if (isInCabin)
        {
            if(ambientAudioSource.isPlaying && ambientAudioSource.clip == normalAmbientSound) 
                ambientAudioSource.Stop();
        }
        if (generator.currentState == Generator.GeneratorState.Stopped)
        {
            if (!ambientAudioSource.isPlaying || ambientAudioSource.clip != horrorAmbientSound)
            {
                ambientAudioSource.clip = horrorAmbientSound;
                ambientAudioSource.Play();
            }
            
        }
        if(generator.currentState == Generator.GeneratorState.Running)
        {
            if (!ambientAudioSource.isPlaying || ambientAudioSource.clip != normalAmbientSound)
            {
                ambientAudioSource.clip = normalAmbientSound;
                ambientAudioSource.Play();
            }
            
        }
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

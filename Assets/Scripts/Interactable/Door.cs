using UnityEngine;
using UnityEngine.AI;

public class Door : Interactable
{
    [SerializeField]
    private GameObject door;

    private bool doorOpen;
    private Animator doorAnimator;
    [SerializeField] private NavMeshObstacle obstacle;

    [SerializeField] private AudioSource audioSource;
    public bool IsOpen => doorOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        doorAnimator = door.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDoor(bool open)
    {
        if(doorOpen == open) return;
        
        doorOpen = open;
        if (doorOpen)
            {
                doorAnimator.Play("Closed");
                audioSource.Play();
            }
            else
            {
                doorAnimator.Play("Opened");
                audioSource.Play();
            }
        obstacle.carving = !doorOpen; 
    }
    
    protected override void Interact()
    {
        // doorOpen = !doorOpen;
        // Debug.Log(doorOpen);
        // Animator doorAnimator = door.GetComponent<Animator>();
        // door.GetComponent<Animator>().SetBool("IsOpen",doorOpen);
        // if (doorOpen)
        // {
        //     doorAnimator.Play("Closed");
        // }
        // else
        // {
        //     doorAnimator.Play("Opened");
        // }
        ToggleDoor(!doorOpen);
        
        // doorAnimator.SetBool("IsOpen", doorOpen);
    }
}

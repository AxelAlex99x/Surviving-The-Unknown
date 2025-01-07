using UnityEngine;

public class Door : Interactable
{
    [SerializeField]
    private GameObject door;

    private bool doorOpen;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        Debug.Log(doorOpen);
        Animator doorAnimator = door.GetComponent<Animator>();
        door.GetComponent<Animator>().SetBool("IsOpen",doorOpen);
        if (doorOpen)
        {
            doorAnimator.Play("Closed");
        }
        else
        {
            doorAnimator.Play("Opened");
        }
        // doorAnimator.SetBool("IsOpen", doorOpen);
    }
}

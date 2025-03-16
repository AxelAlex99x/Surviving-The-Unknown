using System;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private PlayerController pc;
    public AudioSource audioSource;

    public AudioClip dirt;
    public AudioClip runningDirt;
    public AudioClip woodenFloor;
    public AudioClip RunningWoodenFloor;

    public RaycastHit hit;
    public Transform RayStart;
    public float range;
    public LayerMask layerMask;
    private InputManager inputManager;
    private string currentSurfaceTag;
    private string previousSurfaceTag;
    private bool wasSprinting;
    private bool isGrounded;
    
    void Start()
    {
        pc = GetComponent<PlayerController>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        Debug.DrawRay(RayStart.position, Vector3.down * range, Color.red);
        isGrounded = pc.IsGrounded;
        if (!isGrounded)
        {
            audioSource.Stop();
        }
        if (inputManager.playerActions.Movement.IsPressed() && isGrounded)
        {
            Footsteps();
            PlayFootsteps();
            Debug.Log("Audio Working");
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Audio Source is Stopped");
        }
    }

    public void Footsteps()
    {
        if (Physics.Raycast(RayStart.position, Vector3.down, out hit, range, layerMask))
        {
            currentSurfaceTag = hit.collider.tag;
            // if (hit.collider.CompareTag("Ground"))
            // {
            //     audioSource.PlayOneShot(dirt);
            // }
            //
            // if (hit.collider.CompareTag("House"))
            // {
            //     audioSource.PlayOneShot(woodenFloor);
            // }
            Debug.Log(currentSurfaceTag);
        }
    }

    private void PlayFootsteps()
    {
        bool isSprinting = inputManager.playerActions.Sprint.IsPressed();
        
        if (currentSurfaceTag != previousSurfaceTag || isSprinting != wasSprinting)
        {
            audioSource.Stop();
            previousSurfaceTag = currentSurfaceTag;
            wasSprinting = isSprinting;
        }
        
        if (!audioSource.isPlaying)
        {
            switch (currentSurfaceTag)
            {
                case "Ground":
                    if (isSprinting)
                    {
                        audioSource.PlayOneShot(runningDirt); 
                    }
                    else
                    {
                        audioSource.PlayOneShot(dirt);
                    }

                    break;
                case "House":
                    if (isSprinting)
                    {
                        audioSource.PlayOneShot(RunningWoodenFloor);
                    }
                    else
                    {
                        audioSource.PlayOneShot(woodenFloor);
                    }
                    break;
            }
        }
    }
}
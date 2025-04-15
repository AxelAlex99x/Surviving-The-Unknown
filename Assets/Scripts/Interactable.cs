using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Interactable : MonoBehaviour
{
    public string promptMessage;
    private bool isBeingLookedAt;
    public void BaseInteract()
    {
        Interact();
    }

    public void HoldingInteraction()
    {
        HoldInteraction();
    }

    public void ReleasingInteraction()
    {
        ReleaseInteraction();
    }

    public void OnStartLookingAt()
    {
        OnStartLook();
    }
    
    public void OnStopLookingAt()
    {
        OnStopLook();
    }
    
    protected virtual void Interact()
    {
        
    }

    protected virtual void HoldInteraction()
    {
        
    }

    protected virtual void ReleaseInteraction()
    {
        
    }

    protected virtual void OnStartLook()
    {
        if(!isBeingLookedAt)
        isBeingLookedAt = true;
    }
    
    protected virtual void OnStopLook()
    {
        if(!isBeingLookedAt)
        isBeingLookedAt = false;
    }
}

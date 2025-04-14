using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Interactable : MonoBehaviour
{
    public string promptMessage;

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
    protected virtual void Interact()
    {
        
    }

    protected virtual void HoldInteraction()
    {
        
    }

    protected virtual void ReleaseInteraction()
    {
        
    }
}

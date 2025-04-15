using UnityEngine;

public class Fuel : Interactable
{
    protected override void Interact()
    {
        Generator generator = GameObject.FindFirstObjectByType<Generator>();
        if (!generator.PlayerHasFuelCan)
        {
            generator.PlayerHasFuelCan = true;
            Destroy(gameObject);
        }
    }
}

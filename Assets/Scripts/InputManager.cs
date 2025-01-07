using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.PlayerActions playerActions;
    private PlayerController controller;
    private PlayerLook look;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        playerActions = playerInput.Player;

        controller = GetComponent<PlayerController>();
        look = GetComponent<PlayerLook>();

        playerActions.Jump.performed += ctx => controller.Jump();

        playerActions.Sprint.performed += ctx => controller.Sprint();
        playerActions.Sprint.canceled += ctx => controller.Sprint();

        playerActions.Crouch.performed += ctx => controller.Crouch();
        playerActions.Crouch.canceled += ctx => controller.Crouch();
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        controller.ProcessMove(playerActions.Movement.ReadValue<Vector2>());
    }

    // void Update()
    // {
    //     controller.Sprint(playerActions.Sprint.ReadValue<float>() > 0);
    // }
    void LateUpdate() 
    {
        look.ProcessLook(playerActions.Look.ReadValue<Vector2>());
    }
    private void OnEnable() 
    {
        playerActions.Enable();
    }

    private void OnDisable() 
    {
        playerActions.Disable();
    }

}

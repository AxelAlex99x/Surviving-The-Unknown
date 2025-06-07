using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch;
    private bool crounching;
    private bool sprinting;
    private float crouchTimer = 0f;
    [SerializeField]
    private float playerSpeed = 0f;
    [SerializeField]
    private float walkSpeed = 2.5f;
    [SerializeField]
    private float sprintSpeed = 4f;
    [SerializeField]
    private float jumpHeight = 3.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    public bool IsGrounded => isGrounded;
    // public bool IsMoving => walkSpeed > 0.1f;
    // public bool Sprinting => sprinting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;

        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime * 2;
            float p = crouchTimer / 1;
            p *= p;
            if(crounching)
            {
                characterController.height = Mathf.Lerp(characterController.height, 1, p);
            }
            else
            {
                characterController.height = Mathf.Lerp(characterController.height, 2, p);
            }
            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = transform.TransformDirection(moveDirection) * Time.deltaTime * playerSpeed;
        characterController.Move(moveDirection);

        playerVelocity.y += gravityValue * Time.deltaTime;
        
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        
        characterController.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    // public void Sprint(bool isSprinting)
    // {
    //     playerSpeed = isSprinting ? sprintSpeed : walkSpeed;
    // }

    public void Sprint()
    {
        sprinting = !sprinting;
        playerSpeed = sprinting ? sprintSpeed : walkSpeed;
    }

    public void Crouch()
    {
        crounching = !crounching;
        crouchTimer = 0f;
        lerpCrouch = true;
    }
}

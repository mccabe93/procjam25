using UnityEngine;
using UnityEngine.InputSystem;

// Reference:
// https://github.com/D-three/First-Person-Movement-Script-For-Unity/blob/main/First_Person_Movement.cs
public class PlayerMovement : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 MovementInput;
    private Vector3 MouseInput;
    private float RotationX = 0.0f;

    [SerializeField]
    private Transform CameraTransform;

    [SerializeField]
    private CharacterController CharacterController;

    [SerializeField]
    private Transform PlayerTransform;

    [SerializeField]
    private float Speed = 1.0f;

    [SerializeField]
    private float Sensitivity = 1.0f;

    [SerializeField]
    private float Gravity = -9.81f;

    [SerializeField]
    private InputActionReference MoveAction;

    [SerializeField]
    private InputActionReference MouseAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        if (CharacterController.isGrounded)
        {
            Velocity.y = -1f;
        }
        else
        {
            Velocity.y += Gravity * Time.deltaTime;
        }

        Vector2 input = MoveAction.action.ReadValue<Vector2>();
        float verticalSpeed = input.y * Speed;
        float horizontalSpeed = input.x * Speed;

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0f, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;

        MovementInput.x = horizontalMovement.x;
        MovementInput.z = horizontalMovement.z;

        CharacterController.Move(MovementInput * Time.deltaTime);
        CharacterController.Move(Velocity * Time.deltaTime);
    }

    private void MoveCamera()
    {
        MouseInput = MouseAction.action.ReadValue<Vector2>();
        RotationX -= MouseInput.y * Sensitivity;
        RotationX = Mathf.Clamp(RotationX, -90f, 90f);
        PlayerTransform.Rotate(0f, MouseInput.x * Sensitivity, 0f);
        CameraTransform.transform.localRotation = Quaternion.Euler(RotationX, 0f, 0f);
    }
}

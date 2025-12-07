using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Reference:
// https://github.com/D-three/First-Person-Movement-Script-For-Unity/blob/main/First_Person_Movement.cs
public class PlayerSoldierMovement : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 MovementInput;
    private Vector3 MouseInput;
    private float RotationX = 0.0f;

    public AudioClip[] OnMoveSound;

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

    private AudioSource _audioSource;
    private DateTime _lastWalkSoundPlayTime = DateTime.MinValue;
    private TimeSpan _minInterwalkSoundTime = TimeSpan.FromMilliseconds(300);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.1f;
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
        if (input.magnitude > 0 && CharacterController.isGrounded)
        {
            if (
                !_audioSource.isPlaying
                && DateTime.UtcNow - _lastWalkSoundPlayTime > _minInterwalkSoundTime
            )
            {
                _audioSource.clip = OnMoveSound[UnityEngine.Random.Range(0, OnMoveSound.Length)];
                _audioSource.Play();
                _lastWalkSoundPlayTime = DateTime.UtcNow;
            }
        }
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

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _cameraSensitivity = 0.05f;
    [SerializeField] private float _mouseSensitivity = 2f;

    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private Vector3 _mousePosition;
    private float _mouseX;
    private float _mouseY;

    private float _initialYPosition = 0f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
    }

    private void Start()
    {
        _initialYPosition = transform.position.y;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraRotation();

        _mousePosition = Input.mousePosition;
        _mousePosition.x = Mathf.Clamp(_mousePosition.x, 0, Screen.width);
        _mousePosition.y = Mathf.Clamp(_mousePosition.y, 0, Screen.height);
    }

    private void HandleMovement()
    {
        Vector2 keyBoardInput = _moveAction.ReadValue<Vector2>();

        Vector3 moveDirection = transform.forward * keyBoardInput.y + transform.right * keyBoardInput.x;
        moveDirection.y = 0f;

        _characterController.Move(_moveSpeed * Time.deltaTime * moveDirection);

        Vector3 characterControllerPosition = _characterController.transform.position;
        characterControllerPosition.y = _initialYPosition;
        _characterController.transform.position = characterControllerPosition;

        Vector3 position = transform.position;
        position.y = _initialYPosition;
        transform.position = position;
    }

    private void HandleCameraRotation()
    {
        _mouseX = _mousePosition.x - Screen.width / 2f;
        _mouseY = _mousePosition.y - Screen.height / 2f;
        transform.Rotate(_cameraSensitivity * _mouseX * _mouseSensitivity * Time.deltaTime * Vector3.up);

        float verticalRotation = -_mouseY * _mouseSensitivity * _cameraSensitivity * Time.deltaTime;

        Vector3 currentRotation = transform.localEulerAngles;
        float newRotationX = currentRotation.x + verticalRotation;

        if (newRotationX > 180) newRotationX -= 360;
        if (newRotationX < -180) newRotationX += 360;

        newRotationX = Mathf.Clamp(newRotationX, -5f, 20f);

        transform.localEulerAngles = new Vector3(newRotationX, currentRotation.y, currentRotation.z);

        if (_virtualCamera != null)
        {
            var composer = _virtualCamera.GetCinemachineComponent<CinemachineComposer>();
            if (composer != null)
            {
                composer.m_TrackedObjectOffset = new Vector3(composer.m_TrackedObjectOffset.x, 0, composer.m_TrackedObjectOffset.z);
            }
        }
    }
}
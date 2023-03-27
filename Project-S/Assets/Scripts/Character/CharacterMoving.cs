using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMover
{
    public void MovementEnable(bool enable);
}

public class CharacterMoving : MonoBehaviour, IMover
{
    [SerializeField] private CharacterController _characterController;

    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _gravityMultiplier = 100f;
    [SerializeField] private LayerMask _layerMask;

    private PlayerInput _input;

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private bool _movementEnable;

    private void Awake()
    {
        _movementEnable = true;
    }

    public void SetInput(PlayerInput input)
    {
        _input = input;

        //Keyboard
        _input.CharacterControls.Movement.canceled += OnMovementInput;

        //Controller
        _input.CharacterControls.Movement.performed += OnMovementInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    private void Update()
    {
        if (_characterController == null)
            return;

        if (!_movementEnable)
            return;

        _characterController.Move(_currentMovement * Time.deltaTime * _movementSpeed);
        HandleGravity();
        HandleRotation();
    }

    private void HandleMovement()
    {
        /*
        bool isRunning = _animator.GetBool(_runningAnimationParameter);

        if (_isMovementPressed && !isRunning)
        {
            _animator.SetBool(_runningAnimationParameter, true);
        }
        if (!_isMovementPressed && isRunning)
        {
            _animator.SetBool(_runningAnimationParameter, false);
        }
        */
    }

    private void HandleGravity()
    {
        if (_characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            _currentMovement.y = groundedGravity;
        }
        else
        {
            _currentMovement.y -= Time.deltaTime * _gravityMultiplier;
        }
    }

    private void HandleRotation()
    {
        Vector2 screenMousePosition = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(screenMousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, _layerMask))
        {
            Quaternion currentRotation = transform.rotation;
            Vector3 lookPosition = hit.point - transform.position;
            lookPosition.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(lookPosition);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    public void MovementEnable(bool enable)
    {
        _movementEnable = enable;
    }
}

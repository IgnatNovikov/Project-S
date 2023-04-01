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

    [SerializeField] private float _dashSpeed = 30f;
    [SerializeField] private float _dashTime = 1f;

    [SerializeField] private int _evadeLayer;
    [SerializeField] private LayerMask _layerMask;

    private PlayerInput _input;

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;

    private int _defaultLayer;
    private float _movementModificator;
    private bool _isMovementPressed;
    private bool _movementEnable;
    private bool _isDashing;

    private WaitForSeconds _dashWaiter;

    private void Awake()
    {
        _movementEnable = true;
        _isDashing = false;
        _movementModificator = 1f;
        _defaultLayer = gameObject.layer;

        _dashWaiter = new WaitForSeconds(_dashTime);
    }

    public void SetInput(PlayerInput input)
    {
        _input = input;

        //Keyboard
        _input.CharacterControls.Movement.canceled += OnMovementInput;
        _input.CharacterControls.Dash.started += OnDashInput;

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

    private void OnDashInput(InputAction.CallbackContext context)
    {
        if (_isDashing)
            return;

        StartCoroutine(Dashing());
    }

    private void Update()
    {
        if (_characterController == null)
            return;

        if (!_movementEnable)
            return;

        _characterController.Move(_currentMovement * Time.deltaTime * _movementSpeed * _movementModificator);
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

    private IEnumerator Dashing()
    {
        _isDashing = true;
        _movementModificator = _dashSpeed;
        gameObject.layer = _evadeLayer;

        yield return _dashWaiter;

        gameObject.layer = _defaultLayer;
        _movementModificator = 1f;
        _isDashing = false;
    }

    public void MovementEnable(bool enable)
    {
        _movementEnable = enable;
    }
}

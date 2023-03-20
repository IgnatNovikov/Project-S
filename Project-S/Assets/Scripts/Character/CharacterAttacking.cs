using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAttacking : MonoBehaviour
{
    private PlayerInput _input;

    private IAttack _attack;

    private void Start()
    {
        _attack = GetComponentInChildren<IAttack>();
    }

    public void SetInput(PlayerInput input)
    {
        _input = input;

        //Keyboard
        //_input.CharacterControls.Movement.started += OnMovementInput;
        _input.CharacterControls.Attack.started += Attack;

        //Controller
        //_input.CharacterControls.Movement.performed += ;
        //_input.CharacterControls.MousePosition.performed += HandleRotation;

        //_input.CharacterControls.Enable();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (_attack == null)
            return;

        _attack.Attack();
    }
}

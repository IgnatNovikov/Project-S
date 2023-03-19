using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInitializer : MonoBehaviour
{
    [SerializeField] private CharacterMoving _characterMoving;
    //[SerializeField] private CharacterInteraction _characterInteraction;

    private PlayerInput _input;
    private void Awake()
    {
        _input = new PlayerInput();

        if (_characterMoving != null)
            _characterMoving.SetInput(_input);
        //if (_characterInteraction != null)
            //_characterInteraction.SetInput(_input);
    }

    private void OnEnable()
    {
        _input.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _input.CharacterControls.Disable();
    }
}

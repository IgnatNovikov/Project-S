using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _character;

    private Vector3 _position = Vector3.zero;

    private void Start()
    {
        if (_character == null)
            return;

        _position.x = _character.position.x;
        _position.z = _character.position.z;
        _position.y = transform.position.y;
    }

    private void Update()
    {
        if (_character == null)
            return;

        _position.x = _character.position.x;
        _position.z = _character.position.z;

        transform.position = _position;
    }
}

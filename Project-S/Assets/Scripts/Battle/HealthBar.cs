using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float _lastHealth;

    private Camera _camera;

    MaterialPropertyBlock _matBlock;
    private MeshRenderer _meshRenderer;
    private ITakeDamage _damageable;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _matBlock = new MaterialPropertyBlock();
        _damageable = GetComponentInParent<ITakeDamage>();

        _lastHealth = _damageable.GetMaxHealth();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        /*
        if (_damageable.GetCurrentHealth() >= _damageable.GetMaxHealth())
        {
            _meshRenderer.enabled = false;
            return;
        }
        */

        _meshRenderer.enabled = true;
        AlignCamera();
        UpdateParams();
    }

    private void AlignCamera()
    {
        if (_camera == null)
            return;

        /*
        var camXform = _camera.transform;
        var forward = transform.position - camXform.position;
        forward.Normalize();
        var up = Vector3.Cross(forward, camXform.right);

        transform.rotation = Quaternion.LookRotation(forward, up);
        */
        var camXform = _camera.transform;
        var forward = transform.position - camXform.position;
        forward.Normalize();
        var up = Vector3.Cross(forward, camXform.right);
        Quaternion rot = Quaternion.LookRotation(forward, up);
        rot.y = 0;
        rot.z = 0;
        //transform.rotation = Quaternion.LookRotation(forward, up);
        transform.rotation = rot;
    }

    private void UpdateParams()
    {
        if (_lastHealth == _damageable.GetCurrentHealth())
            return;

        //Debug.Log(_damageable.GetCurrentHealth());

        _lastHealth = _damageable.GetCurrentHealth();

        _meshRenderer.GetPropertyBlock(_matBlock);
        _matBlock.SetFloat("_Fill", _damageable.GetCurrentHealth() / (float)_damageable.GetMaxHealth());
        _meshRenderer.SetPropertyBlock(_matBlock);
    }
}

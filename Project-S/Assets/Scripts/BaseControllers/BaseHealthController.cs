using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITakeDamage
{
    public void TakeDamage(float damage);
    public float GetCurrentHealth();
    public float GetMaxHealth();
    public void SetOnDamageAction(UnityAction<float> action);
}

public class BaseHealthController : MonoBehaviour, ITakeDamage
{
    [Header("Settings")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _knocbackPower;

    [Header("Components")]
    [SerializeField] private HealthBar _healthBar;

    private float _health;

    private UnityAction<float> _onDamageAction;

    private void Start()
    {
        _health = _maxHealth;
    }

    protected virtual void ChangeHealth(float value)
    {
        if (value < 0)
            return;

        _health = value;
    }

    public virtual void TakeDamage(float damage)
    {
        _onDamageAction?.Invoke(_knocbackPower);

        _health -= damage;

        Debug.Log(_health);

        if (_health <= 0)
        {
            _health = 0;
            Death();
        }
    }

    public float GetCurrentHealth()
    {
        return _health;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetOnDamageAction(UnityAction<float> action)
    {
        _onDamageAction = action;
    }

    protected virtual void Death()
    {

    }
}

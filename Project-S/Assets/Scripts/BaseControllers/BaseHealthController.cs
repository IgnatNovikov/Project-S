using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    public void TakeDamage(float damage);
}

public class BaseHealthController : MonoBehaviour, ITakeDamage
{
    private float _health;

    protected virtual void ChangeHealth(float value)
    {
        if (value < 0)
            return;

        _health = value;
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log(damage);

        _health -= damage;

        if (_health <= 0)
            Death();
    }

    protected virtual void Death()
    {

    }
}

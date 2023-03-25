using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IAttack
{
    void Attack();
}

public class BaseAttack : MonoBehaviour, IAttack
{
    [SerializeField] private AttackConfig _attackConfig;
    private List<Collider> _attackColliders;
    private List<Collider> _hittedColliders = new List<Collider>();

    private WaitForSeconds _attackTime;

    private bool _isAttacking = false;

    private void Start()
    {
        _attackColliders = GetComponentsInChildren<Collider>().ToList();

        _attackTime = new WaitForSeconds(_attackConfig.AttackTime);

        _hittedColliders.Clear();

        _isAttacking = false;
    }

    public void Attack()
    {
        if (_isAttacking)
            return;

        SwitchColliders(true);

        StartCoroutine(AttackProcess());
    }

    private IEnumerator AttackProcess()
    {
        _isAttacking = true;

        yield return _attackTime;

        SwitchColliders(false);

        _hittedColliders.Clear();

        _isAttacking = false;
    }

    private void SwitchColliders(bool enable)
    {
        foreach (Collider collider in _attackColliders)
        {
            collider.enabled = enable;
        }
    }

    private void DoDamage(ITakeDamage enemy)
    {
        enemy.TakeDamage(_attackConfig.Damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hittedColliders.Contains(other))
            return;

        ITakeDamage enemy = other.GetComponent<ITakeDamage>();

        if (enemy == null)
            return;

        //Debug.Log(other.name);
        _hittedColliders.Add(other);

        DoDamage(enemy);
    }
}
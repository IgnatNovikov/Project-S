using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemy
{
    public void Init(Transform playerTransform);
}

public class EnemyController : MonoBehaviour, IEnemy
{
    [SerializeField] private float _lookRadius = 10f;

    private Transform _target;
    private NavMeshAgent _agent;
    private ITakeDamage _healthController;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _healthController = GetComponent<ITakeDamage>();

        _healthController.SetOnDamageAction(KnocbackAction);
    }

    private void Update()
    {
        if (_target == null)
            return;

        float distance = Vector3.Distance(_target.position, transform.position);

        if (distance > _lookRadius)
            return;

        _agent.SetDestination(_target.position);

        if (distance > _agent.stoppingDistance)
            return;

        //attack
        //face target
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _lookRadius);
    }

    public void Init(Transform playerTransform)
    {
        _target = playerTransform;
    }

    private void KnocbackAction(float power)
    {
        GetComponent<Rigidbody>().AddForce(-transform.forward * power);
    }
}

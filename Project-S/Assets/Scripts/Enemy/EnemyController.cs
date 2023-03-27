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

    [Header("Animations")]
    [SerializeField] private string _knockBackAnimationName;

    private Transform _target;
    private NavMeshAgent _agent;
    private ITakeDamage _healthController;
    private Rigidbody _rigidbody;
    private Animator _animator;

    private bool _isKnockBack;
    private float _knockBackPower;
    private float _knockBackReduce;
    private WaitForSeconds _knockBackTime;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _healthController = GetComponent<ITakeDamage>();

        _healthController.SetOnDamageAction(KnocbackAction);
    }

    private void FixedUpdate()
    {
        if (_isKnockBack)
        {
            KnockBacking();
        }

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

    private void KnockBacking()
    {
        _rigidbody.MovePosition(transform.position + -transform.forward * Time.deltaTime * _knockBackPower);
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

    private void KnocbackAction(float power, float time)
    {
        _isKnockBack = true;
        _knockBackPower = power;
        _knockBackTime = new WaitForSeconds(time);

        StartCoroutine(IsKnockBacking());

        /*
        GetComponent<Rigidbody>().AddForce(-transform.forward * power, ForceMode.Acceleration);
        Debug.DrawLine(transform.position, - transform.forward * power, Color.red);
        */
    }

    private IEnumerator IsKnockBacking()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _animator.SetTrigger(_knockBackAnimationName);

        yield return _knockBackTime;

        _isKnockBack = false;
        _agent.isStopped = false;
    }
}

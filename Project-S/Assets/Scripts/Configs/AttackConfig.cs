using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Test/Battle/Attacks/Create Attack Config", order = 0)]
public class AttackConfig : ScriptableObject
{
    [SerializeField] private float _attackTime;
    public float AttackTime => _attackTime;

    [SerializeField] private float _damage;
    public float Damage => _damage;
}

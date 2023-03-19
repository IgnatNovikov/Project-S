using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private List<IEnemy> _enemies;

    private void Awake()
    {
        _enemies = GetComponentsInChildren<IEnemy>().ToList();

        foreach (IEnemy enemy in _enemies)
        {
            enemy.Init(_player);
        }
    }
}

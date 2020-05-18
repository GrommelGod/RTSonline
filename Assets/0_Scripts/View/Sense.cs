using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    [SerializeField]
    private LayerMask _enemy;

    private Collider[] _enemiesInRange;
    private List<GameObject> _enemiesInRangeList = new List<GameObject>();

    private RangerViewStats _stats;
    private EnemyViewStats _enemyStats;
    private UnitMovement _unitMovement;
    private UnitAttack _unitAttack;

    private GameObject _closestEnemy;
    public bool _playerAttack;

    private void Awake()
    {
        _stats = GetComponent<RangerViewStats>();
        _enemyStats = GetComponent<EnemyViewStats>();
        _unitMovement = GetComponent<UnitMovement>();
        _unitAttack = GetComponent<UnitAttack>();

        if (_stats == null)
        {
            return;
        }
        if (_enemyStats == null)
        {
            return;
        }
    }

    public void FixedUpdate()
    {
        if (!_playerAttack)
        {
            if (_stats != null)
            {
                _enemiesInRange = Physics.OverlapSphere(transform.position, _stats._ranger._rangerRange, _enemy);
            }
            if (_enemyStats)
            {
                _enemiesInRange = Physics.OverlapSphere(transform.position, _enemyStats._enemy._enemyRange, _enemy);
            }

            foreach (Collider enemy in _enemiesInRange)
            {
                _enemiesInRangeList.Add(enemy.gameObject);
            }

            if (_enemiesInRangeList.Count != 0)
            {
                if (GetClosestEntity() != null)
                {
                    _closestEnemy = GetClosestEntity().gameObject;
                    _unitMovement.EnemyTarget(_closestEnemy);
                    _unitAttack.EnemyTarget(_closestEnemy);
                }
            }
        }
        else
        {
            ClearList();
        }
    }

    private Collider GetClosestEntity()
    {
        if (_enemiesInRange.Length != 0)
        {
            return _enemiesInRange[0];
        }
        else
        {
            return null;
        }
    }

    public void ClearList()
    {
        _enemiesInRangeList.Clear();
    }
}

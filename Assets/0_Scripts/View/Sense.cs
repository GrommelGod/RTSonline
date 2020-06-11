using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sense : MonoBehaviour
{
    [SerializeField]
    private LayerMask _enemy;
    private List<GameObject> _enemiesInRangeList = new List<GameObject>();

    private Unit _stats;

    private GameObject _closestEnemy;

    public event Action<GameObject> OnEnemyDetected;

    private void Awake()
    {
        _stats = GetComponent<Unit>();
        OnEnemyDetected += GetComponent<UnitMovement>().OnEnemyDetected;
        OnEnemyDetected += GetComponent<UnitAttack>().OnEnemyDetected;
    }

    private void Start()
    {
        InvokeRepeating("CheckUnit", 0, .1f);
    }

    private void CheckUnit()
    {
        var _enemiesInRange = Physics.OverlapSphere(transform.position, _stats.UnitRange, _enemy);

        if (_enemiesInRange.Length == 0)
            return;

        _closestEnemy = _enemiesInRange.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).First().gameObject;
        OnEnemyDetected?.Invoke(_closestEnemy);
    }
}

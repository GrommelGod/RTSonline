using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public bool _selected;
    public bool _clickOnEnemy;
    private bool _holdPos;

    public NavMeshAgent _navMesh;

    private Vector3 _positionUnit;

    private float _distanceEnemy;

    private GameObject _currentEnemy;

    [SerializeField]
    private GameObject _selectionUI;

    private Sense _sense;
    private Unit _unitsStats;

    private void Awake()
    {
        _sense = GetComponent<Sense>();
        _navMesh = GetComponent<NavMeshAgent>();
        _positionUnit = transform.position;
        _unitsStats = GetComponent<Unit>();
    }

    internal void OnEnemyDetected(GameObject enemy)
    {
        _currentEnemy = enemy;
    }

    private void Start()
    {
        if (_unitsStats != null)
        {
            _navMesh.SetDestination(_positionUnit);
        }
    }

    private void Update()
    {
        #region Range between units
        if (_currentEnemy != null)
        {
            _distanceEnemy = Vector3.Distance(transform.position, _currentEnemy.transform.position);
        }
        #endregion

        #region Selection UI
        if (_selected)
        {
            _navMesh.SetDestination(_positionUnit);
            if (_selectionUI != null)
            {
                _selectionUI.SetActive(true);
            }
        }
        else
        {
            if (_selectionUI != null)
            {
                _selectionUI.SetActive(false);
            }
        }
        #endregion

        if (_selected)
        {
            ///When the player hits Z, the unit stops
            if (Input.GetButtonDown("Stop"))
            {
                _navMesh.SetDestination(transform.position);
            }
            ///When the player hits E, the unit won't follow units getting out of sight
            if (Input.GetButtonDown("Hold"))
            {
                _navMesh.SetDestination(transform.position);
                _holdPos = true;
            }
        }

        #region Move Toward Enemy
        if (!_selected && !_clickOnEnemy)
        {
            if (_currentEnemy != null)
            {
                transform.LookAt(_currentEnemy.transform);
                if (_unitsStats != null)
                {
                    if (_distanceEnemy > _unitsStats.UnitRange && !_holdPos)
                    {
                        _navMesh.SetDestination(_currentEnemy.transform.position);
                    }
                    else
                    {
                        _navMesh.SetDestination(transform.position);
                    }
                }
            }
        }
        else if (_selected && !_clickOnEnemy)
        {

        }
        else
        {
            if (_currentEnemy != null)
            {
                _holdPos = false;
                transform.LookAt(_currentEnemy.transform);
                if (_distanceEnemy > _unitsStats.UnitRange)
                {
                    _navMesh.SetDestination(_currentEnemy.transform.position);
                }
                else
                {
                    _navMesh.SetDestination(transform.position);
                }
            }
        }
        #endregion
    }

    public void AtThisPosition(Vector3 position)
    {
        _positionUnit = position;
    }
}

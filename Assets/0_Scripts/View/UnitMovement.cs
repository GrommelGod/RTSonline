using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public bool _selected;
    public bool _clickOnEnemy;

    public NavMeshAgent _navMesh;

    private Vector3 _positionUnit;

    private float _distanceEnemy;

    private GameObject _currentEnemy;

    [SerializeField]
    private GameObject _selectionUI;

    private Sense _sense;
    private RangerViewStats _unitsStats;
    private EnemyViewStats _enemyStats;

    private void Awake()
    {
        _sense = GetComponent<Sense>();
        _navMesh = GetComponent<NavMeshAgent>();
        _positionUnit = transform.position;
        _unitsStats = GetComponent<RangerViewStats>();
        _enemyStats = GetComponent<EnemyViewStats>();

        if (_unitsStats == null)
        {
            return;
        }
        if (_enemyStats == null)
        {
            return;
        }
    }

    private void Start()
    {
        if(_unitsStats != null)
        {
            _navMesh.SetDestination(_positionUnit);
        }
    }

    private void Update()
    {
        //Debug.Log(_currentEnemy);
        //Debug.Log(_distanceEnemy);

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

        #region Move Toward Enemy
        if (!_selected && !_clickOnEnemy)
        {
            _sense._playerAttack = false;
            if (_currentEnemy != null)
            {
                transform.LookAt(_currentEnemy.transform);
                if (_unitsStats != null)
                {
                    if (_distanceEnemy > _unitsStats._ranger._rangerRange)
                    {
                        _navMesh.SetDestination(_currentEnemy.transform.position);
                    }
                    else
                    {
                        _navMesh.SetDestination(transform.position);
                    }
                }

                if (_enemyStats != null)
                {
                    if (_distanceEnemy > _enemyStats._enemy._enemyRange)
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
                transform.LookAt(_currentEnemy.transform);
                _sense._playerAttack = true;
                if (_distanceEnemy > _unitsStats._ranger._rangerRange)
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

    public void EnemyTarget(GameObject enemy)
    {
        _currentEnemy = enemy;
    }
}

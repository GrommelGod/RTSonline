using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    private GameObject _currentEnemy;
    private RangerViewStats _stats;
    private EnemyViewStats _enemyStats;
    private float _distanceEnemy;

    private bool _reload = false;

    [SerializeField]
    private LayerMask _enemy;

    [SerializeField]
    private GameObject _shotgun;

    [SerializeField]
    private GameObject _blood;

    private void Awake()
    {
        _stats = GetComponent<RangerViewStats>();
        _enemyStats = GetComponent<EnemyViewStats>();

        if (_stats == null)
        {
            return;
        }
        if (_enemyStats == null)
        {
            return;
        }
    }

    void Update()
    {
        if (_currentEnemy != null)
        {
            _distanceEnemy = Vector3.Distance(transform.position, _currentEnemy.transform.position);

            if (_stats != null)
            {
                if (_distanceEnemy <= _stats._ranger._rangerRange)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(_shotgun.transform.position, _shotgun.transform.forward, out hit, _stats._ranger._rangerRange, _enemy))
                    {
                        if (!_reload)
                        {
                            hit.collider.GetComponent<EnemyViewStats>()._enemy.TakeDamage(5);
                            Instantiate(_blood, hit.point, Quaternion.identity);
                            _reload = true;
                        }
                        else
                        {
                            StartCoroutine(Reload());
                        }
                    }
                }
            }

            if (_enemyStats != null)
            {
                if (_distanceEnemy <= _enemyStats._enemy._enemyRange)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(_shotgun.transform.position, _shotgun.transform.forward, out hit, _enemyStats._enemy._enemyRange, _enemy))
                    {
                        if (!_reload)
                        {
                            hit.collider.GetComponent<RangerViewStats>()._ranger.TakeDamage(5);
                            Instantiate(_blood, hit.point, Quaternion.identity);
                            _reload = true;
                        }
                        else
                        {
                            StartCoroutine(Reload());
                        }
                    }
                }
            }
        }


    }
    public void EnemyTarget(GameObject target)
    {
        _currentEnemy = target;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(5f * Time.deltaTime);
        _reload = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    private Unit _stats;

    [SerializeField]
    private LayerMask _enemy;
    [SerializeField]
    private GameObject _shotgun;
    [SerializeField]
    private GameObject _blood;

    private void Awake()
    {
        _stats = GetComponent<Unit>();
    }

    internal void OnEnemyDetected(GameObject enemy)
    {
        StartCoroutine(Shoot(enemy));
    }

    private IEnumerator Shoot(GameObject enemy)
    {
        while (true)
        {
            if (enemy == null)
                yield break;

            float _distanceEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (_distanceEnemy > _stats.UnitRange)
                yield break;

            RaycastHit hit;
            if (Physics.Raycast(_shotgun.transform.position, _shotgun.transform.forward, out hit, _stats.UnitRange, _enemy))
            {
                DoHit(enemy, hit.transform.position);
                yield return new WaitForSeconds(10f * Time.deltaTime);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void DoHit(GameObject enemy, Vector3 hitPoint)
    {
        Instantiate(_blood, hitPoint, Quaternion.identity);
        enemy.GetComponent<Entity>().TakeDamage(5);
    }
}

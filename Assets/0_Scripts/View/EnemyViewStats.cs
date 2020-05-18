using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewStats : MonoBehaviour
{
    public Enemy _enemy = new Enemy(50, Team.TeamBlue, 10, 8);

    private UnitStorage _unitStorage;

    private void Awake()
    {
        _unitStorage = FindObjectOfType<UnitStorage>();
        _unitStorage.AddUnit(gameObject, true);
    }

    private void Update()
    {
        //Debug.Log(_enemy._enemyLife);
        //Not working.
        if (_enemy._enemyLife <= 0)
        {
            _unitStorage.RemoveUnit(gameObject, true);

            Destroy(gameObject);
        }
    }
}

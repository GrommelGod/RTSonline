using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerViewStats : MonoBehaviour
{
    public Ranger _ranger = new Ranger(50, Team.TeamRed, 10, 8);

    private UnitStorage _unitStorage;

    private void Awake()
    {
        _unitStorage = FindObjectOfType<UnitStorage>();
        _unitStorage.AddUnit(gameObject, false);
    }

    private void Update()
    {
        //Debug.Log(_ranger._rangerLife);
        //Not working.
        if (_ranger._rangerLife <= 0)
        {
            _unitStorage.RemoveUnit(gameObject, false);

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStorage : MonoBehaviour
{
    private Player _player = new Player();

    private List<GameObject> _units = new List<GameObject>();
    private List<GameObject> _enemyUnits = new List<GameObject>();
    private List<GameObject> _buildings = new List<GameObject>();
    private List<GameObject> _enemyBuildings = new List<GameObject>();

    public List<GameObject> Units { get => _units; }

    public void AddUnit(GameObject unit, bool _enemy)
    {
        if (!_enemy)
        {
            _units.Add(unit);
            _player.CreateUnit();
        }
        else
        {
            _enemyUnits.Add(unit);
        }
    }

    public void RemoveUnit(GameObject unit, bool _enemy)
    {
        if (!_enemy)
        {
            _units.Remove(unit);
            _player.DeleteUnit();
        }
        else
        {
            _enemyUnits.Remove(unit);
        }
    }

    public void AddBuilding(GameObject building, bool _enemy)
    {
        if (!_enemy)
        {
            _buildings.Add(building);
            _player.CreateBuilding();
        }
        else
        {
            _enemyBuildings.Remove(building);
        }
    }

    public void RemoveBuilding(GameObject building, bool _enemy)
    {
        if (!_enemy)
        {
            _buildings.Remove(building);
            _player.DeleteBuilding();
        }
        else
        {
            _enemyBuildings.Remove(building);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStorage : MonoBehaviour
{
    private int _unitID;

    private Dictionary<int, Entity> _units = new Dictionary<int, Entity>();
    private Dictionary<int, Entity> _buildings = new Dictionary<int, Entity>();

    public Dictionary<int, Entity> Units { get => _units; }

    public void AddUnit(Entity unit)
    {
        _unitID++;
        unit.UnitID = _unitID;
        _units.Add(unit.UnitID, unit);
    }

    public void RemoveUnit(int unitID)
    {
        _units.Remove(unitID);
    }

    public void AddBuilding(Entity build)
    {
        _buildings.Add(build.UnitID, build);
    }

    public void RemoveBuilding(int buildID)
    {
        _buildings.Remove(buildID);
    }
}

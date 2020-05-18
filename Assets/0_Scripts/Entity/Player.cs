using UnityEngine;

public class Player : ITeamOwner
{
    private Team _team;

    private int _unitCount;
    private int _buildingCount;

    public int UnitCount { get => _unitCount; set => _unitCount = value; }

    public Team GetTeam()
    {
        return _team;
    }

    public void CreateUnit()
    {
        _unitCount++;
    }
    public void DeleteUnit()
    {
        _unitCount--;
    }
    public void CreateBuilding()
    {
        _buildingCount++;
    }
    public void DeleteBuilding()
    {
        _buildingCount--;
    }
}

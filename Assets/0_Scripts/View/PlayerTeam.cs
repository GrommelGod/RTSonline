using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeam : MonoBehaviour, ITeamOwner
{
    [SerializeField]
    private Team _team;

    public Team GetTeam()
    {
        return _team;
    }

    private void Update()
    {
        Debug.Log(_team);
    }
}

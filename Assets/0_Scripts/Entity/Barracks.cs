using UnityEngine;

public class Barracks : Entity
{
    public int _barrackLife;
    public Team _Team;
   
    public Barracks(Team team, int lifePoints, int speed, int range) : base(team, lifePoints, speed, range)
    {
        _barrackLife = lifePoints;
        _Team = team;
    }
}

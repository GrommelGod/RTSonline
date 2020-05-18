using UnityEngine;

public class Enemy : Entity
{
    public int _enemyLife;
    public int _enemySpeed;
    public int _enemyRange;
    private Team _Team;

    public Enemy(int life, Team team, int speed, int range) : base(team, life, speed, range)
    {
        _enemySpeed = speed;
        _enemyRange = range;
        _enemyLife = life;
        _Team = team;
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("here");
        _lifePoints -= damage;
    }
}

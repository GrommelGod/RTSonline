public class Ranger : Entity
{
    public int _rangerLife;
    public int _rangerSpeed;
    public int _rangerRange;
    private Team _Team;

    public Ranger(int life, Team team, int speed, int range) : base(team, life, speed, range) 
    {
        _rangerSpeed = speed;
        _rangerRange = range;
        _rangerLife = life;
        _Team = team;
    }

    public override void TakeDamage(int damage)
    {
        _lifePoints -= damage;
    }
}

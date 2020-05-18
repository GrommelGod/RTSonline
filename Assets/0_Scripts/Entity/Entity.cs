public abstract class Entity : ITeamOwner
{
    protected Team _team;
    protected int _lifePoints;
    protected int _speed;
    protected int _range;

    protected Entity(Team team, int lifePoints, int speed, int range)
    {
        _team = team;
        _lifePoints = lifePoints;
        _speed = speed;
        _range = range;
    }

    public Team GetTeam()
    {
        return _team;
    }

    public virtual void TakeDamage(int damage)
    {
        _lifePoints -= damage;
    }
}

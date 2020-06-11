using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour, ITeamOwner
{
    [SerializeField]
    private Team _team;
    [SerializeField]
    private int lifePoints;
    private int _unitID;

    [SerializeField]
    protected Image _lifeBar;

    protected int maxHealth;

    public int LifePoints { get => lifePoints; private set => lifePoints = value; }
    public int UnitID { get => _unitID; set => _unitID = value; }

    public Team GetTeam()
    {
        return _team;
    }

    private void Awake()
    {
        maxHealth = LifePoints;
    }

    public void TakeDamage(int damage)
    {
        LifePoints -= damage;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            _lifeBar.enabled = true;
        }
        else
        {
            _lifeBar.enabled = false;
        }

        _lifeBar.fillAmount = ((float)LifePoints / maxHealth);
    }
}

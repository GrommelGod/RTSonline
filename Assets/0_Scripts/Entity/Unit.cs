using UnityEngine;
using UnityEngine.UI;

public class Unit : Entity
{
    [SerializeField]
    private int _unitRange;

    public int UnitRange { get => _unitRange; private set => _unitRange = value; }

    private void Update()
    {
        if (LifePoints <= 0)
        {
            Network.GetInstance().RemoveUnitFromServer(UnitID);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : Entity
{
    private UnitStorage _unitStorage;
    private ProductionUnit _production;
    private Entity _barrackEntity;

    private void Awake()
    {
        _barrackEntity = GetComponent<Entity>();
        _production = FindObjectOfType<ProductionUnit>();
    }

    private void Update()
    {
        if (LifePoints <= 0)
        {
            _production.SelectedBuildings.Remove(_barrackEntity);
            Network.GetInstance().RemoveBuilding(_barrackEntity.UnitID);
            Destroy(gameObject);
        }

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

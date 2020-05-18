using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionUnit : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _building, _ground;
    [SerializeField]
    private GameObject _productionButton;

    private List<GameObject> _selectedBuildings = new List<GameObject>();

    private void Update()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _building))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    ClearBuildingsList();
                }
                _selectedBuildings.Add(hit.collider.gameObject);
                hit.collider.GetComponent<BarrackView>()._canProduce = true;
                _productionButton.SetActive(true);
            }
        }
        else if(Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask("UI")))
        {

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClearBuildingsList();
                _productionButton.SetActive(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                foreach (GameObject item in _selectedBuildings)
                {
                    //Not working. The rally point doesn't go on the wanted position
                    item.GetComponent<BarrackView>().RallyPointPosition(hit.point);
                }
            }
        }
    }

    private void ClearBuildingsList()
    {
        foreach (GameObject item in _selectedBuildings)
        {
            item.GetComponent<BarrackView>()._canProduce = false;
        }
        _selectedBuildings.Clear();
    }

    public void ButtonRanger()
    {
        foreach (GameObject item in _selectedBuildings)
        {
            item.GetComponent<BarrackView>().OrderToSpawnRanger();
        }
    }
}

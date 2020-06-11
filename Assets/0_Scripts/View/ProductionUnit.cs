using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductionUnit : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _building, _ground;
    [SerializeField]
    private GameObject _button;

    private List<Entity> _selectedBuildings = new List<Entity>();

    public List<Entity> SelectedBuildings { get => _selectedBuildings; set => _selectedBuildings = value; }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _building))
            {
                _button.SetActive(true);
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    ClearBuildingsList();
                }
                _selectedBuildings.Add(hit.collider.gameObject.GetComponent<Entity>());
                hit.collider.GetComponent<BarrackView>()._canProduce = true;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
            {
                _button.SetActive(false);
                ClearBuildingsList();
            }
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
        {
            if (Input.GetMouseButtonDown(1))
            {
                foreach (Entity item in _selectedBuildings)
                {
                    item.GetComponent<BarrackView>().RallyPointPosition(hit.point);
                }
            }
        }
    }

    private void ClearBuildingsList()
    {
        foreach (Entity item in _selectedBuildings)
        {
            item.GetComponent<BarrackView>()._canProduce = false;
        }
        _selectedBuildings.Clear();
    }

    public void ButtonUnit()
    {
        foreach (Entity item in _selectedBuildings)
        {
            item.GetComponent<BarrackView>().ButtonUnit();
        }
    }
}

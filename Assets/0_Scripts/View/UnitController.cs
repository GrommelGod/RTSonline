using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private LayerMask _units, _ground, _enemy;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private RectTransform _rect;

    private UnitStorage _unitStorage;
    private Sense _sense;

    private Vector2 _startDragPos;
    private Vector2 _endDragPos;
    private Vector2 _mousePosDrag1;
    private Vector2 _mousePosDrag2;

    private List<GameObject> _clickedUnit = new List<GameObject>();

    void Awake()
    {
        _unitStorage = FindObjectOfType<UnitStorage>();
        _rect.gameObject.SetActive(false);
    }

    void Update()
    {
        #region LeftClick
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raySelectBox;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out raySelectBox, Mathf.Infinity))
            {
                _startDragPos = Input.mousePosition;
            }

            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _units))
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    ClearUnitsList();
                }
                _clickedUnit.Add(hit.collider.gameObject);
                hit.collider.GetComponent<UnitMovement>()._selected = true;
            }
            else
            {
                ClearUnitsList();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _rect.gameObject.SetActive(false);

            if (_startDragPos != _endDragPos)
            {
                DragSelect();
            }
        }

        if (Input.GetMouseButton(0))
        {
            #region DragVisual
            if (!_rect.gameObject.activeInHierarchy)
            {
                _rect.gameObject.SetActive(true);
            }

            _endDragPos = Input.mousePosition;

            Vector2 squareStart = _startDragPos;

            Vector2 center = (squareStart + _endDragPos) / 2f;

            _rect.position = center;

            float sizeX = Mathf.Abs(squareStart.x - _endDragPos.x);
            float sizeY = Mathf.Abs(squareStart.y - _endDragPos.y);

            _rect.sizeDelta = new Vector2(sizeX, sizeY);
            #endregion

            ///TODO Fix that. The drag selection doesn't work.
            var _rectSelect = _camera.ScreenToWorldPoint(_rect.sizeDelta);

            foreach (GameObject item in _unitStorage.Units)
            {
                if (_rectSelect.x.Equals(item.transform.position.x))
                {
                    _clickedUnit.Add(item);
                    item.GetComponent<UnitMovement>()._selected = true;
                }
            }
        }
        #endregion

        #region RightClick
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit1;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit1, Mathf.Infinity, _enemy))
            {
                //Debug.Log("enemy target");

                if (_clickedUnit != null)
                {
                    foreach (GameObject item in _clickedUnit)
                    {
                        item.GetComponent<UnitMovement>()._clickOnEnemy = true;
                        item.GetComponent<UnitMovement>().EnemyTarget(hit1.collider.gameObject);
                        item.GetComponent<UnitAttack>().EnemyTarget(hit1.collider.gameObject);
                    }
                }
            }
            else if (Physics.Raycast(ray, out hit1, Mathf.Infinity, _ground))
            {
                if (_clickedUnit != null)
                {
                    foreach (GameObject item in _clickedUnit)
                    {
                        item.GetComponent<UnitMovement>().AtThisPosition(hit1.point);
                        item.GetComponent<UnitMovement>()._clickOnEnemy = false;
                    }
                }
            }
        }
        #endregion
    }

    private void DragSelect()
    {
        Rect _rectangle = new Rect(_startDragPos.x, _startDragPos.y, _endDragPos.x - _startDragPos.x, _endDragPos.y - _endDragPos.y);
    }

    private void ClearUnitsList()
    {
        foreach (GameObject item in _clickedUnit)
        {
            item.GetComponent<UnitMovement>()._selected = false;
        }
        _clickedUnit.Clear();
    }
}

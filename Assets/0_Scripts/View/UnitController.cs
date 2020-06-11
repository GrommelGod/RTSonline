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
    [SerializeField]
    private UnitStorage _unitStorage;

    private Vector2 _startDragPos;

    private List<Entity> _clickedUnit = new List<Entity>();

    public List<Entity> ClickedUnit { get => _clickedUnit; private set => _clickedUnit = value; }

    void Awake()
    {
        _rect.gameObject.SetActive(false);
    }

    void Update()
    {
        #region LeftClick
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Network.GetInstance()._localPlayer.ColorIndex == 0)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _units))
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        ClearUnitsList();
                    }
                    _clickedUnit.Add(hit.collider.gameObject.GetComponent<Entity>());
                    hit.collider.GetComponent<UnitMovement>()._selected = true;
                }
                else
                {
                    ClearUnitsList();
                }

                _startDragPos = Input.mousePosition;
            }
            else
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _enemy))
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        ClearUnitsList();
                    }
                    _clickedUnit.Add(hit.collider.gameObject.GetComponent<Entity>());
                    hit.collider.GetComponent<UnitMovement>()._selected = true;
                }
                else
                {
                    ClearUnitsList();
                }

                _startDragPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _rect.gameObject.SetActive(false);

            Vector2 _rectMin = _rect.anchoredPosition - _rect.sizeDelta / 2;
            Vector2 _rectMax = _rect.anchoredPosition + _rect.sizeDelta / 2;

            foreach (KeyValuePair<int, Entity> item in _unitStorage.Units)
            {
                var posScreen = _camera.WorldToScreenPoint(item.Value.gameObject.transform.position);

                if (posScreen.x > _rectMin.x && posScreen.x < _rectMax.x &&
                    posScreen.y > _rectMin.y && posScreen.y < _rectMax.y)
                {
                    _clickedUnit.Add(item.Value);
                    item.Value.GetComponent<UnitMovement>()._selected = true;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            #region DragVisual
            if (!_rect.gameObject.activeInHierarchy)
            {
                _rect.gameObject.SetActive(true);
            }

            var _endDragPos = Input.mousePosition;

            float sizeX = _endDragPos.x - _startDragPos.x;
            float sizeY = _endDragPos.y - _startDragPos.y;

            _rect.sizeDelta = new Vector2(Mathf.Abs(sizeX), Mathf.Abs(sizeY));
            _rect.anchoredPosition = _startDragPos + new Vector2(sizeX / 2, sizeY / 2);
            #endregion
        }
        #endregion

        #region RightClick
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit1;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit1, Mathf.Infinity, _enemy))
            {
                if (_clickedUnit != null)
                {
                    foreach (Entity item in _clickedUnit)
                    {
                        item.GetComponent<UnitMovement>()._clickOnEnemy = true;
                        item.GetComponent<UnitMovement>().OnEnemyDetected(hit1.collider.gameObject);
                        item.GetComponent<UnitAttack>().OnEnemyDetected(hit1.collider.gameObject);
                    }
                }
            }
            else if (Physics.Raycast(ray, out hit1, Mathf.Infinity, _ground))
            {
                if (_clickedUnit != null)
                {
                    foreach (Entity item in _clickedUnit)
                    {
                        item.GetComponent<UnitMovement>().AtThisPosition(hit1.point);
                        item.GetComponent<UnitMovement>()._clickOnEnemy = false;
                    }
                }
            }
        }
        #endregion
    }

    private void ClearUnitsList()
    {
        foreach (Entity item in _clickedUnit)
        {
            item.GetComponent<UnitMovement>()._selected = false;
        }
        _clickedUnit.Clear();
    }
}

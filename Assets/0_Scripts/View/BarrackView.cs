using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackView : MonoBehaviour
{
    private Team _Team;
    [SerializeField]
    private GameObject _rangerPrefab, _enemyPrefab;
    [SerializeField]
    private LineRenderer _rallyLine;
    [SerializeField]
    private GameObject _rallyPoint;

    private UnitStorage _unitStorage;
    private GatherResources _resources;

    public bool _canProduce;
    private bool _unitInLine;
    private Transform _barrackPos;
    private Vector3 _rallyPointPos;
    private float _processTimer = 8f;

    private void Awake()
    {
        _Team = GetComponent<Barrack>().GetTeam();
        _resources = FindObjectOfType<GatherResources>();
        _unitStorage = FindObjectOfType<UnitStorage>();
    }

    private void Start()
    {
        _barrackPos = gameObject.transform;
    }

    private void Update()
    {
        _rallyLine.SetPosition(0, _barrackPos.position);
        _rallyLine.SetPosition(1, _rallyPoint.transform.position);
        _rallyPoint.transform.position = _rallyPointPos;

        if (_canProduce)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!_unitInLine && _resources.Resources >= 20)
                {
                    OrderToSpawnUnit();
                    _resources.Resources -= 20;
                    _unitInLine = true;
                }
            }
        }
    }

    public void ButtonUnit()
    {
        if (!_unitInLine && _resources.Resources >= 20)
        {
            OrderToSpawnUnit();
            _resources.Resources -= 20;
            _unitInLine = true;
        }
    }

    public void RallyPointPosition(Vector3 rally)
    {
        _rallyPointPos = rally;
    }

    private void SpawnUnit()
    {
        if (_Team == Team.TeamRed)
        {
            var instantiated = Instantiate(_rangerPrefab, transform.position, Quaternion.identity);
            _unitStorage.AddUnit(instantiated.GetComponent<Entity>());
            Network.GetInstance().UnitOnServer(instantiated.GetComponent<Entity>().UnitID);
            instantiated.GetComponent<UnitMovement>().AtThisPosition(_rallyPointPos);
        }
        else
        {
            var instantiated = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            _unitStorage.AddUnit(instantiated.GetComponent<Entity>());
            Network.GetInstance().UnitOnServer(instantiated.GetComponent<Entity>().UnitID);
            instantiated.GetComponent<UnitMovement>().AtThisPosition(_rallyPointPos);
        }
    }

    public void OrderToSpawnUnit()
    {
        StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(_processTimer);
        SpawnUnit();
        _unitInLine = false;
    }
}

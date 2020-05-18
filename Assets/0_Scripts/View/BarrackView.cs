using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackView : MonoBehaviour
{
    private Transform _barrackPos;

    public bool _canProduce;
    [SerializeField]
    private GameObject _rangerPrefab;

    private Vector3 _rallyPointPos;
    public GameObject _rallyPoint;

    [SerializeField]
    private LineRenderer _rallyLine;

    private float _processTimer = 10f;

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
                OrderToSpawnRanger();
            }
        }
    }

    public void RallyPointPosition(Vector3 rally)
    {
        _rallyPointPos = rally;
    }

    private void SpawnRanger()
    {
        Instantiate(_rangerPrefab, _rallyPoint.transform.position, Quaternion.identity);
    }

    public void OrderToSpawnRanger()
    {
        StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(_processTimer);
        SpawnRanger();
    }
}

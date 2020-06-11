using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherResources : MonoBehaviour
{
    [SerializeField]
    private Text _resourceText;
    [SerializeField]
    private UnitStorage _unitStorage;
    [SerializeField]
    private Text _unitCount;

    private bool _gathering;
    private int _timer = 0;
    private int _timerMax = 200;

    private int _resources = 0;

    public int Resources { get => _resources; set => _resources = value; }

    public void PlayerResources()
    {
        _resourceText.text = _resources.ToString();
    }

    private void Update()
    {
        if (!_gathering)
        {
            Gathering();
        }
        else
        {
            _timer++;
            if (_timer >= _timerMax)
            {
                _gathering = false;
                _timer = 0;
            }
        }

        _unitCount.text = _unitStorage.Units.Count.ToString();
    }

    private void Gathering()
    {
        _resources += 5;
        _resourceText.text = _resources.ToString();
        _gathering = true;
    }
}

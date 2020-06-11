using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesBonus : MonoBehaviour
{
    [SerializeField]
    private int _bonus;
    [SerializeField]
    private GatherResources _resources;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            _resources.Resources += _bonus;
        }
        Destroy(gameObject);
    }
}


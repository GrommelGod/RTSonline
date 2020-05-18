using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SelfDestruction());
    }

    private IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}

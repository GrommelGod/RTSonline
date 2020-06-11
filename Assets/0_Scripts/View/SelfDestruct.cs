using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int _bounce;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _bounce = Random.Range(-6, 6);
        StartCoroutine(SelfDestruction());
        _rigidbody.AddForce(new Vector3(_bounce, 1f, _bounce), ForceMode.Impulse);
    }

    private IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(4.5f);
        Destroy(gameObject);
    }
}

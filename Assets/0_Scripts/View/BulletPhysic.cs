using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysic : MonoBehaviour
{
    [SerializeField]
    private int _speed;
    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _body.AddForce(transform.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}

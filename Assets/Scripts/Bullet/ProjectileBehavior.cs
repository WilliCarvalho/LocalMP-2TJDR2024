using System;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField] private float moveSpeed = 10f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigidbody.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}

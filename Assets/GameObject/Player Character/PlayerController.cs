using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotateSpeed = 5;


    void Start()
    {

    }


    void Update()
    {
        Move();
        Rotate();
    }



    private void Move()
    {
        float vertical = Input.GetAxis("Vertical");
        _rb.velocity = new Vector3(transform.forward.x * vertical * _moveSpeed, _rb.velocity.y, transform.forward.z * vertical * _moveSpeed);
    }

    private void Rotate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(0f, _rb.rotation.x + horizontal * _rotateSpeed, 0f)
    );
    }

}

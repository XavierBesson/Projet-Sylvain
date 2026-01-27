using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotateSpeed = 5;

    [SerializeField] private bool _oldSchoolMove = false;
    [SerializeField] private float stepDistance = 1.5f;   
    [SerializeField] private float snapRotation = 45f;
    [SerializeField]  private LayerMask obstacles;

    public Camera Camera { get => _camera; set => _camera = value; }

    void Start()
    {
        //GameManager.Instance.Player = this;
    }


    void Update()
    {
        Move();
        Rotate();
    }

    #region Déplacement
    private void Move()
    {
        if (_oldSchoolMove == false)
        {
            float vertical = Input.GetAxis("Vertical");
            _rb.velocity = new Vector3(transform.forward.x * vertical * _moveSpeed, _rb.velocity.y, transform.forward.z * vertical * _moveSpeed);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
                TryMove(this.transform.forward);
        }
    }

    void TryMove(Vector3 dir)
    {
        if (!Physics.Raycast(transform.position, dir, stepDistance, obstacles))
        {
            transform.position += dir * stepDistance;
        }
    }
    private void Rotate()
    {
        if (_oldSchoolMove == false)
        {
            float horizontal = Input.GetAxis("Horizontal");
            _rb.MoveRotation(_rb.rotation * Quaternion.Euler(0f, _rb.rotation.x + horizontal * _rotateSpeed, 0f));
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.Rotate(0f, -snapRotation, 0f);
            }

            // Rotation droite
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.Rotate(0f, snapRotation, 0f);
            }

        }
    }
    #endregion Déplacement


}

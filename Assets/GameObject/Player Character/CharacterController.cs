using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private LayerMask _obstacles;


    [SerializeField] private float _interactRange = 100000f;
    [SerializeField] private LayerMask _interactile;
    [SerializeField] private Vector3 _mousePosition;

    [SerializeField] private float _hp;
    [SerializeField] private float _hpMax = 5f;


    public Camera Camera { get => _camera; set => _camera = value; }
    public float Hp { get => _hp; set => _hp = value; }

    void Start()
    {
        GameManager.Instance.Player = this;
        _hp = _hpMax;
    }


    void Update()
    {
        Move();
        Rotate();
       // MousePosition();
        MouseClic();
        _mousePosition = Input.mousePosition;
        
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
        if (!Physics.Raycast(transform.position, dir, stepDistance, _obstacles))
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

    void MousePosition()
    {

    }

     public void HpUpdate( float value) 
    {
        Hp = value;
        if (Hp > 0f)
        {
            Debug.Log("J'ai actuellement" + Hp + "Pv");
        }
        else 
        {
            Debug.Log("DEAD");
                }
    }

    public void Hpdamage(float damage)
    {
        Hp = Hp - damage;
        Debug.Log("J'ai actuellement" + Hp + "Pv");
    }



    void MouseClic()

    { if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clic"); 
           RaycastHit hit;
          Ray ray = GameManager.Instance.Player.Camera.ScreenPointToRay(Input.mousePosition);


            Physics.Raycast(ray, out hit, _interactile);

            //Debug.Log(hit.collider.gameObject);

                    if (hit.collider.gameObject.GetComponentInParent<Door>() != null)
                    {
                        hit.collider.gameObject.GetComponentInParent<Door>().RevealHint();
                    }
                    //Rajouter des else If pour tous les autres objets interactible 
                }

            }
        }


    


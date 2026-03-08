using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using static Coffee.UIExtensions.UIParticleAttractor;

public class CharacterController : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private bool _oldSchoolMove = false;

    [Header("Déplacement Moderne")]
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotateSpeed = 5;

    [Header("Déplacement Rétro")]
    [SerializeField] private float _stepDistance = 1.5f;
    [SerializeField] private float _snapRotation = 45f;
    [SerializeField] private float _rotationRate = 2f;
    [SerializeField] private LayerMask _obstacles;
    [SerializeField] private LayerMask _stairs;
    private float _targetRotation;
    private float _currentRotation;

    [Header("Mouse Interractions")]
    [SerializeField] private float _interactRange = 100000f;
    [SerializeField] private LayerMask _interactile;
    [SerializeField] private Vector3 _mousePosition;
    private UIObject _currentUIObject = null;

    [Header("HP")]
    [SerializeField] private float _hp;
    [SerializeField] private float _hpMax = 5f;
    [SerializeField] private bool _hpRegen = false;
    private bool _isDead = false;


    public Camera Camera { get => _camera; set => _camera = value; }
    public float Hp { get => _hp;
        set {_hp = value;
            GameManager.Instance.PlayerHUDController.ChangeHPDisplay(Hp);
            if (Hp <= 0f) { Debug.Log("DEAD"); }
        } }
    public UIObject CurrentUIObject { get { return _currentUIObject; } set => _currentUIObject = value; }


    void Start()
    {
        _hp = _hpMax;
        _currentRotation = transform.rotation.eulerAngles.y;
        _targetRotation = _currentRotation;
    }


    void Update()
    {
        if (_isDead == false)
        {
            Move();
            Rotate();
        }
        // MousePosition();
        MouseClic();
        _mousePosition = Input.mousePosition;
       // HpRegen();
        
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
            if (Input.GetKeyDown(KeyCode.S))
                TryMove(-this.transform.forward);
        }
    }

    void TryMove(Vector3 dir)
    {
        if (!Physics.Raycast(transform.position, dir, _stepDistance, _obstacles))
        {
            transform.position += dir * _stepDistance;
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
                _targetRotation -= _snapRotation;
                //transform.Rotate(0f, -_snapRotation, 0f);
                if (_currentUIObject != null)
                    CurrentUIObject.Move();
            }

            // Rotation droite
            if (Input.GetKeyDown(KeyCode.D))
            {
                _targetRotation += _snapRotation;
                //transform.Rotate(0f, _snapRotation, 0f);
                if (_currentUIObject != null)
                    CurrentUIObject.Move();
            }

            if (_currentRotation < _targetRotation)
            {
                transform.Rotate(0, _rotationRate, 0);
                _currentRotation += _rotationRate;
            }
            if (_currentRotation > _targetRotation)
            {
                transform.Rotate(0, -_rotationRate, 0);
                _currentRotation -= _rotationRate;
            }

            Debug.Log(_currentRotation);
            Debug.Log(_targetRotation);
        }
    }
    #endregion Déplacement

    void MousePosition()
    {

    }


    #region HP

    public void Hpdamage(float damage)
    {
        Hp = Hp - damage;
        if (Hp > 0f)
        {
            Debug.Log("J'ai actuellement" + Hp + "Pv");
        }
        else
        {
            Debug.Log("Je suis mort");
            GameManager.Instance.PlayerHUDController.PlayerIsDead(); 

            //Autre évenement de mort (audio et autre)
        }
    }

    public void HpRegen()
    {if (_hpRegen)
        {
            Hp = Hp + 0.1f;
        }
    }
    /*  public void ActivateRegen(bool isActive)
      {
          _hpRegen =isActive;

          Debug.Log(Hp); 
      }*/

    #endregion HP


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


    


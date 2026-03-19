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
    [SerializeField] private bool _moving = true;

    [Header("Déplacement Moderne")]
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotateSpeed = 5;

    [Header("Déplacement Rétro")]
    [SerializeField] private float _stepDistance = 1.5f;
    [SerializeField] private float _snapRotation = 45f;
    [SerializeField] private float _rotationRate = 2f;
    [SerializeField] private LayerMask _obstacles;
    [SerializeField] private LayerMask _floor;
    [SerializeField] private LayerMask _stairs;
    private float _targetRotation;
    private float _currentRotation;

    [Header("Mouse Interractions")]
    [SerializeField] private float _interactRange = 100000f;
    [SerializeField] private LayerMask _interactile;
    private UIObject _currentUIObject = null;

    [Header("HP")]
    private float _hp;
    [SerializeField] private float _hpMax = 5f;
    private bool _hpRegen = false;
    private bool _isDead = false;
    private bool _isStairs = false;


    public Camera Camera { get => _camera; set => _camera = value; }
    public float Hp { get => _hp;
        set {_hp = value;
            if (GameManager.Instance.PlayerHUDController != null)
            {
                GameManager.Instance.PlayerHUDController.ChangeHPDisplay(Hp);
                HPVisual();
            }
            if (Hp <= 0f) { Death(); }
        } }
    public UIObject CurrentUIObject { get { return _currentUIObject; } set => _currentUIObject = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }


    void Start()
    {
        Hp = _hpMax;
        GameManager.Instance.PlayerHUDController.ChangeHPDisplay(Hp);
        _currentRotation = transform.rotation.eulerAngles.y;
        _targetRotation = _currentRotation;
    }


    void Update()
    {
        if (_moving)
        {
            if (!IsDead)
            {
                Move();
                Rotate();
            }
        }
        // MousePosition();
        MouseClic();
        // HpRegen();

        if (GameManager.Instance.PlayerHUDController != null)
            GameManager.Instance.PlayerHUDController.ChangeHPDisplay(Hp);
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
          
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                TryMove(this.transform.forward);
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                TryMove(-this.transform.forward);
        }
    }


    void TryMove(Vector3 dir)
    {
        Vector3 dir2 = -this.transform.up;
        if (!Physics.Raycast(transform.position, dir, _stepDistance + 0.5f, _obstacles, QueryTriggerInteraction.Ignore) && Physics.Raycast(transform.position, dir2, 1.1f, _floor, QueryTriggerInteraction.Ignore))
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
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _targetRotation -= _snapRotation;
                //transform.Rotate(0f, -_snapRotation, 0f);
                if (_currentUIObject != null)
                    CurrentUIObject.Move();
            }

            // Rotation droite
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
        }
    }

    #endregion Déplacement


    #region HP

    public void Hpdamage(float damage, bool inStairs = false)
    {
        Hp = Hp - damage;
        if (Hp > 0f)
        {
            GameManager.Instance.PlayerHUDController.TakeDammageStart();
        }
        _isStairs = inStairs;
    }


    public void HPVisual()
    {
        if (Hp > _hpMax / 3)
        {
            GameManager.Instance.PlayerHUDController.LowHpVisuelFeedback(false);
        }
        else
        {
            GameManager.Instance.PlayerHUDController.LowHpVisuelFeedback(true);
        }
    }


    public void IsInStairs(bool stairs)
    {
        _isStairs = stairs;
    }


    public void Death()
    {
        Debug.Log("Je suis mort");
       
        GameManager.Instance.PlayerHUDController.PlayerIsDead(_isStairs);
        IsDead = true;
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
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clic"); 
            RaycastHit hit;
            Ray ray = GameManager.Instance.Player.Camera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit, _interactile);

            //Debug.Log(hit.collider.gameObject);

            if (hit.collider.gameObject.GetComponent<ClicableObject>() != null) 
            {
                print(hit.collider.gameObject.GetComponent<ClicableObject>());
                hit.collider.gameObject.GetComponent<ClicableObject>().DisplayText();
            }
        }
    }


}
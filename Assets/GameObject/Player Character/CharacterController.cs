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
    [SerializeField] private Vector3 _positionToMove = Vector3.zero;

    [Header("Déplacement Moderne")]
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotateSpeed = 5;

    [Header("Déplacement Rétro")]
    [SerializeField] private float _stepDistance = 1.5f;
    [SerializeField] private float _snapRotation = 45f;
    [SerializeField] private float _rotationRate = 2f;
    [SerializeField] private LayerMask _obstaclesLayer;
    [SerializeField] private LayerMask _floorLayer;
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

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioClip _rotateSound;
    [SerializeField] private AudioClip _damageSound;


    private bool _isGrounded = false;

    public Camera Camera { get => _camera; set => _camera = value; }
    public float Hp { get => _hp;
        set {_hp = Mathf.Clamp(value, 0, HpMax);
            if (GameManager.Instance.PlayerHUDController != null)
            {
                GameManager.Instance.PlayerHUDController.ChangeHPDisplay(Hp);
                HPVisual();
            }
            if (Hp <= 0f) { Death(); }
        } }
    public UIObject CurrentUIObject { get { return _currentUIObject; } set => _currentUIObject = value; }
    public bool IsDead { get => _isDead; set { _isDead = value; IsMoving = value; } }
    public bool IsMoving
    {
        get => _moving;
        set
        {
            _moving = value;
            if (value) { GameManager.Instance.GameLoop += Actions; }
            else { GameManager.Instance.GameLoop -= Actions; }
        }
    }
    public float HpMax { get => _hpMax; set => _hpMax = value; }
    

    void Start()
    {
        _positionToMove = transform.position;
        Hp = HpMax;
        _currentRotation = transform.rotation.eulerAngles.y;
        _targetRotation = _currentRotation;

        if (IsMoving)
        {
            GameManager.Instance.PlayerHUDController.ChangeHPDisplay(Hp);
            GameManager.Instance.GameLoop += Actions;
        }
    }


    void Update()
    {

    }


    #region Déplacement

    private void Actions()
    {
        Vector3 dir2 = -this.transform.up;
        if (Physics.Raycast(transform.position, dir2, 1.1f, _floorLayer, QueryTriggerInteraction.Ignore) && _isGrounded)
        {
            Move();
        }
        else { _positionToMove = transform.position; Move(); }
        ActivateRotation();
        MouseClic();

        if (GameManager.Instance.PlayerHUDController != null)
            GameManager.Instance.PlayerHUDController.ChangeHPDisplay(Hp);
    }



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


    private void MoveLerp()
    {
        transform.position = Vector3.Lerp(transform.position, _positionToMove, Time.deltaTime*60 ); 

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _isGrounded)
            TryMoveLerp(this.transform.forward);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && _isGrounded)
            TryMoveLerp(-this.transform.forward);
    }

    private void TryMoveLerp(Vector3 dir)
    {
        Vector3 dir2 = -this.transform.up;
        _isGrounded = Physics.Raycast(transform.position, -Vector3.up, 1.1f, _floorLayer, QueryTriggerInteraction.Ignore);
        if (!Physics.Raycast(_positionToMove, dir, _stepDistance + 0.5f, _obstaclesLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 freeTarget = _positionToMove + dir * (_stepDistance + 0.5f);
            //Vector3 rayOrigin = new Vector3(freeTarget.x, freeTarget.y + 2f, freeTarget.z);

            if (Physics.Raycast(freeTarget, Vector3.down, out RaycastHit groundHit, 10f, _obstaclesLayer, QueryTriggerInteraction.Ignore))
            {
                Vector3 groundPoint = groundHit.point;

                _positionToMove += dir * _stepDistance;
                _positionToMove.y = groundPoint.y + 1;
                
            }
            else
                _positionToMove += dir * _stepDistance;
        }
    }

    void TryMove(Vector3 dir)
    {
        _isGrounded = Physics.Raycast(transform.position, -Vector3.up, 1.1f, _floorLayer, QueryTriggerInteraction.Ignore);

        if (!Physics.Raycast(transform.position, dir, _stepDistance + 0.5f, _obstaclesLayer, QueryTriggerInteraction.Ignore) && 
            _isGrounded)
        {
            GameManager.PlaySounds(_audioSource, _moveSound);
            transform.position += dir * _stepDistance;
        }
    }


    private void ActivateRotation()
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
                GameManager.PlaySounds(_audioSource, _rotateSound);
                _targetRotation -= _snapRotation;
                IsMoving = false;
                GameManager.Instance.GameLoop += Rotate;
                if (_currentUIObject != null)
                    CurrentUIObject.Move();
            }

            // Rotation droite
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameManager.PlaySounds(_audioSource, _rotateSound);
                _targetRotation += _snapRotation;
                IsMoving = false;
                GameManager.Instance.GameLoop += Rotate;
                //transform.Rotate(0f, _snapRotation, 0f);
                if (_currentUIObject != null)
                    CurrentUIObject.Move();
            }
        }
    }



    private void Rotate()
    {
        if (_currentRotation < _targetRotation)
        {
            transform.Rotate(0, _rotationRate, 0);
            _currentRotation += _rotationRate;
        }
        else if (_currentRotation > _targetRotation)
        {
            transform.Rotate(0, -_rotationRate, 0);
            _currentRotation -= _rotationRate;
        }
        else
        {
            _currentRotation = _targetRotation;
            GameManager.Instance.GameLoop -= Rotate;
            IsMoving = true;
        }
    }


    #endregion Déplacement


    #region HP

    public void Hpdamage(float damage, bool inStairs = false)
    {
        Hp = Hp - damage;
        GameManager.Instance.Door.SpikeDamage(damage);
        if (Hp > 0f)
        {
            GameManager.Instance.PlayerHUDController.TakeDammageStart();
            GameManager.PlaySounds(_audioSource, _damageSound);
        }
        _isStairs = inStairs;
    }


    public void HPVisual()
    {
        if (Hp > HpMax / 3)
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
       
        GameManager.Instance.Die();
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


    private void OnDestroy()
    {
        GameManager.Instance.GameLoop -= Actions;
    }


}
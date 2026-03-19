using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EUIObject
{
    NONE,
    ENGRENAGE,
    HEALTHBAR,
    OBJECTIF,
    PLATFORM,
    VOLUMEBAR,
    EASYSWORD,
    MEDIUMSWORD,
    HARDSWORD
}


public class UIObject : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] private float _dragDistance = 3;
    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private EUIObject _objectType = EUIObject.NONE;
    [SerializeField] private GameObject _highlight;
    private bool _isDragging = false;
    private CharacterController _playerCharacter = null;
    private DetachableUi _detachableUI = null;
    private float _despawnTimer = 0;
    private float _maxDespawnTimer = 3;

    public CharacterController PlayerCharacter { get => _playerCharacter; set => _playerCharacter = value; }
    public bool IsDragging { get => _isDragging; set => _isDragging = value; }
    public DetachableUi DetachableUI { get => _detachableUI; set => _detachableUI = value; }
    public EUIObject ObjectType { get => _objectType; set => _objectType = value; }



    private void Start()
    {
        PlayerCharacter = GameManager.Instance.Player;
        HighlightObject(false);
    }


    #region Dragging

    public virtual void Update()
    {
        if (IsDragging && Input.GetMouseButtonUp(1))
            Undrag();
    }


    public virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
            Drag();
    }
   

    public void Drag()
    {
        IsDragging = true;
        _rb.isKinematic = true;
        GameManager.Instance.GameLoop -= DespawnTimer;
        GameManager.Instance.Player.CurrentUIObject = this;
        GameManager.Instance.GameLoop += Move;
    }


    public virtual void Undrag()
    {
        _rb.isKinematic = false;
        IsDragging = false;
        GameManager.Instance.GameLoop += DespawnTimer;
        _despawnTimer = 0;
        GameManager.Instance.Player.CurrentUIObject = null;
        GameManager.Instance.GameLoop -= Move;
    }


    private void DespawnTimer()
    {
        _despawnTimer += Time.deltaTime;
        if (_despawnTimer >= _maxDespawnTimer)
        {
            ReturnToUI();
        }
    }

    #endregion Dragging


    public void Move()
    {
        Ray ray = GameManager.Instance.Player.Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, GameManager.Instance.Player.Camera.nearClipPlane + _dragDistance, _raycastMask))
        {
            _rb.MovePosition(hit.point - ray.direction * 0.2f);
        }
        else
        {
            Vector3 mousePos = GameManager.Instance.Player.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.Player.Camera.nearClipPlane + _dragDistance));
            _rb.MovePosition(mousePos);
        }

        _rb.rotation = Quaternion.Euler(-(45 * (Input.mousePosition.y - 540) / 540), 70 * (Input.mousePosition.x - 960) / 960 + PlayerCharacter.transform.rotation.eulerAngles.y, 0);
    }


    #region Despaw

    public void Despawn()
    {
        GameManager.Instance.GameLoop -= Move;
        GameManager.Instance.GameLoop -= DespawnTimer;
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!IsDragging && collision.gameObject.layer == 9)
            ReturnToUI();
    }


    public void ReturnToUI()
    {
        DetachableUI.ResetPosition();
        Despawn();
    }

    #endregion Despaw

    public void HighlightObject(bool highlight)
    {
        if (_highlight != null)
            _highlight.SetActive(highlight);
    }
}

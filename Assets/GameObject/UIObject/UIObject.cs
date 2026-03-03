using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] private float _dragDistance = 3;
    [SerializeField] private Collider _collider = null;
    private bool _isDragging = false;
    private CharacterController _playerCharacter = null;
    [SerializeField] private LayerMask _raycastMask;

    public Collider Collider { get => _collider; set => _collider = value; }
    public CharacterController PlayerCharacter { get => _playerCharacter; set => _playerCharacter = value; }
    public bool IsDragging { get => _isDragging; set => _isDragging = value; }

    private void Start()
    {
        PlayerCharacter = GameManager.Instance.Player;
    }

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
        GameManager.Instance.Player.CurrentUIObject = this;
        GameManager.Instance.GameLoop += Move;
    }

    public virtual void Undrag()
    {
        _rb.isKinematic = false;
        IsDragging = false;
        GameManager.Instance.Player.CurrentUIObject = null;
        GameManager.Instance.GameLoop -= Move;
    }


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



    public void Despawn()
    {
        GameManager.Instance.GameLoop -= Move;
        Destroy(gameObject);
    }


}

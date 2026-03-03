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

    void Start()
    {
        _playerCharacter = GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnMouseOver()
    {
       if (Input.GetMouseButtonDown(1))
       {
           _isDragging = true;
           _rb.isKinematic = true;
           GameManager.Instance.Player.CurrentUIObject = this;
       }
        else if (Input.GetMouseButtonUp(1))
        {
            _rb.isKinematic = false;
            _isDragging = false;
            GameManager.Instance.Player.CurrentUIObject = null;
        }
        
    }


    private void OnMouseDrag()
    {
        if (_isDragging)
        {
            Move();
        }
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

        _rb.rotation = Quaternion.Euler(-(45 * (Input.mousePosition.y - 540) / 540), 70 * (Input.mousePosition.x - 960) / 960 + _playerCharacter.transform.rotation.eulerAngles.y, 0);

        /*
        Vector3 mousePos = GameManager.Instance.Player.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.Player.Camera.nearClipPlane + _dragDistance));
        _rb.MovePosition(mousePos);
        


        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + _cameraHeight, transform.position.z), _actualVC.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _cameraDistance, _raycastMask, QueryTriggerInteraction.Ignore))
        {
            _actualVC.transform.position = hit.point;
        }
        else
        {
            _actualVC.transform.position = transform.position + _actualVC.rotation * new Vector3(0, _cameraHeight, _cameraDistance);
        }*/
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] private float _dragDistance = 3;
    [SerializeField] private LayerMask _mask = 0;
    private bool _isDragging = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnMouseDown()
    {
       
        _isDragging = true;
        _rb.isKinematic = true;
        
    }

    private void OnMouseUp()
    {
        _rb.isKinematic = false;
        _isDragging = false;
    }




    private void OnMouseDrag()
    {
        if (_isDragging)
        {
            Vector3 mousePos = GameManager.Instance.Player.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.Player.Camera.nearClipPlane + _dragDistance));
            _rb.MovePosition(mousePos);
            _rb.rotation = Quaternion.Euler(-(45 * (Input.mousePosition.y - 540) / 540), 70 * (Input.mousePosition.x - 960) / 960, 0);
        }
    }



}

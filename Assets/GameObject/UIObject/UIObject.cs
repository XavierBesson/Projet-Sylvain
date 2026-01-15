using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _dragDistance = 5;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _rb.isKinematic = true;
            GameManager.Instance.GameLoop += MoveOnMouse;
        }


        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _rb.isKinematic = false;
            GameManager.Instance.GameLoop -= MoveOnMouse;
        }
            
    }



    public void MoveOnMouse()
    {
        Ray ray = GameManager.Instance.Player.Camera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPos;

        if (Physics.Raycast(ray, out RaycastHit hit, _dragDistance, 3))
        {
            targetPos = hit.point + transform.forward;
        }
        else
        {
            targetPos = ray.origin + ray.direction * _dragDistance;
        }
        _rb.MovePosition(targetPos);

    }



}

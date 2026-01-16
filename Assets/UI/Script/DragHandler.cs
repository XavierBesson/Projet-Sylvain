using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    [SerializeField] private DetachableUi _uiElement;
    [SerializeField] private float _shakeRate = 1.0f;
    [SerializeField] private float _maxDistanceBeforeBreaking = 20;
    [SerializeField] private Vector2 _initialPosition = Vector2.zero;
    private bool _dragging = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_uiElement != null)
        {
            ShakeLogic();
        }
    }

    private void ShakeLogic()
    {
        if (Input.GetKey(KeyCode.Mouse0) && _dragging)
        {
            float distance = Vector2.Distance(_initialPosition, Input.mousePosition);
            if (_uiElement.Attached)
            {
                ShakeElement(distance);
                if (distance >= _maxDistanceBeforeBreaking)
                    _uiElement.Attached = false;
            }
            else
            {
                _uiElement.transform.position = Input.mousePosition;
            }
        }
        else if (_uiElement.Attached)
            _uiElement.transform.position = _initialPosition;
    }

    public void EnterDrag()
    {
        Debug.Log("enter drag");
        _dragging = true;
    }

    public void Drag()
    {
        
    }

    public void ExitDrag()
    {
        Debug.Log("exit drag");
        _dragging = false;
    }

    private void ShakeElement(float distance)
    {
        Vector2 shakePosition = _initialPosition + Random.insideUnitCircle * _shakeRate * distance;
        _uiElement.transform.position = shakePosition;
    }

    private void OnDrawGizmosSelected()
    {
        float shakeRadius = _maxDistanceBeforeBreaking;
        Gizmos.DrawSphere(_initialPosition, shakeRadius);
        Gizmos.color = Color.yellow;
    }
}

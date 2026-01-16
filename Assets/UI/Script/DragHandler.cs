using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _uiElement;
    [SerializeField] private float _shakeRate = 1.0f;
    [SerializeField] private float _maxDistanceBeforeBreaking = 20;
    [SerializeField] private Vector2 _initialPosition = Vector2.zero;
    [SerializeField] private bool _attached = true;
    private bool _dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        _uiElement = GetComponent<RectTransform>();
        _initialPosition = _uiElement.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Drag();
        if (Input.GetKey(KeyCode.Mouse0) && _dragging)
        {
            float distance = Vector2.Distance(_initialPosition, Input.mousePosition);
            if (_attached)
            {
                ShakeElement(distance);
                if (distance >= _maxDistanceBeforeBreaking)
                    _attached = false;
            }
            else
            {
                _uiElement.position = Input.mousePosition;
            }
        }
        else if (_attached)
            _uiElement.position = _initialPosition;
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
        _uiElement.position = shakePosition;
    }

    private void OnDrawGizmosSelected()
    {
        float shakeRadius = _maxDistanceBeforeBreaking;
        Gizmos.DrawSphere(_initialPosition, shakeRadius);
        Gizmos.color = Color.yellow;
    }
}

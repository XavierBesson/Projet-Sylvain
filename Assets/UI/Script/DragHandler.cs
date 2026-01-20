using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode _holdBreakMode = KeyCode.None;
    [Header("Settings")]
    [SerializeField] private DetachableUi _uiElement = null;
    [SerializeField] private DetachableUi[] _detachableUis = null;
    [SerializeField] private float _shakeRate = 1.0f;
    [SerializeField] private float _maxDistanceBeforeBreaking = 20;
    [SerializeField] private float _maxToSnapBack = 10;
    private bool _dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        _detachableUis = FindObjectsOfType<DetachableUi>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(_holdBreakMode))
        {
            if (_uiElement.GetComponent<Selectable>() != null)
                _uiElement.GetComponent<Selectable>().interactable = false;
            ShakeLogic();
        }
        else
            _uiElement.GetComponent<Selectable>().interactable = true;
    }

    private void ShakeLogic()
    {
        if (_uiElement != null && _dragging)
        {

            if (_uiElement.GetComponent<Selectable>() != null)
                _uiElement.GetComponent<Selectable>().interactable = false;

            float distance = Vector2.Distance(_uiElement.InitialPosition, Input.mousePosition);
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
            _uiElement.SetToInitialPosition();
    }

    private void SnapBackLogic()
    {
        float distance = Vector2.Distance(_uiElement.InitialPosition, _uiElement.transform.position);
        if (distance < _maxToSnapBack)
        {
            _uiElement.Attached = true;
            _uiElement.SetToInitialPosition();
        }
    }

    public void EnterDrag(DetachableUi ui)
    {
        Debug.Log("enter drag");
        _dragging = true;
        _uiElement = ui;
    }

    public void Drag()
    {
        
    }

    public void ExitDrag()
    {
        Debug.Log("exit drag");
        SnapBackLogic();
        _dragging = false;
        _uiElement = null;
    }

    private void ShakeElement(float distance)
    {
        Vector2 shakePosition = _uiElement.InitialPosition + Random.insideUnitCircle * _shakeRate * distance;
        _uiElement.transform.position = shakePosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (_dragging)
        {
            float shakeRadius = _maxDistanceBeforeBreaking;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_uiElement.InitialPosition, shakeRadius);
            Gizmos.color = Color.red;
            float snapRadius = _maxToSnapBack;
            Gizmos.DrawWireSphere(_uiElement.InitialPosition, snapRadius);
        }
    }
}

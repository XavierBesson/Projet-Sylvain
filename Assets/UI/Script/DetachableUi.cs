using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetachableUi : MonoBehaviour
{
    [SerializeField] private GameObject _uiObject = null;
    [SerializeField] private GameObject _uiSlot = null;
    [SerializeField] private bool _attached = true;
    private Vector2 _initialPosition = Vector2.zero;

    public bool Attached
    {
        get
        {
            return _attached;
        }
        set
        {
            _attached = value;
        }
    }

    public GameObject UiObject => _uiObject;

    public Vector2 InitialPosition
    {
        get { return _initialPosition; }
        set
        {
            _initialPosition = value;
            if(_uiSlot)
                _uiSlot.transform.position = _initialPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        if (_uiSlot)
            Instantiate(_uiSlot, InitialPosition, Quaternion.identity, transform);
    }

    public void SetToInitialPosition()
    {
        transform.position = InitialPosition;
    }
}

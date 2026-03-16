using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetachableUi : MonoBehaviour
{
    [SerializeField] private GameObject _uiObject = null;
    [SerializeField] private GameObject _uiSlot = null;
    [SerializeField] private Transform _uiSlotParent = null;
    private bool _attached = true;
    [SerializeField] private Sprite _imageRestante = null;
    [SerializeField] private Vector3 _slotScale = Vector3.one;

    private Vector2 _initialPosition = Vector2.zero;

    public GameObject UiObject { get => _uiObject; set => _uiObject = value; }

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


    public Vector2 InitialPosition
    {
        get { return _initialPosition; }
        set
        {
            _initialPosition = value;
            if (_uiSlot)
            {
                _uiSlot.transform.position = _initialPosition;
                _uiSlot.transform.localScale = _slotScale;
            }
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        if (_uiSlot)
            _uiSlot = Instantiate(_uiSlot, InitialPosition, Quaternion.identity, _uiSlotParent);
       
    }


    public void SetToInitialPosition()
    {
        transform.position = InitialPosition;
    }

    public void SetUiSlotState(bool breaked)
    {
        _uiSlot.GetComponent<UiSlot>().SetSprite(breaked,_imageRestante);

    }


    public void ResetPosition()
    {
        gameObject.SetActive(true);
        Attached = true;
        SetToInitialPosition();
        SetUiSlotState(false);
    }

}

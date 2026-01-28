using UnityEngine;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode _holdBreakMode = KeyCode.None;
    private bool _breakMode = false;
    [SerializeField] private GameObject _breakParticle = null;

    [Header("Settings")]
    [SerializeField] private DetachableUi _uiElement = null;
    [SerializeField] private DetachableUi[] _detachableUis = null;
    [SerializeField] private float _minShake = 0.1f;
    [SerializeField] private float _shakeRate = 1.0f;
    [SerializeField] private float _maxDistanceBeforeBreaking = 20;
    [SerializeField] private float _maxToSnapBack = 10;
    private float _uiScale = 1.0f;
    private bool _dragging = false;

    #region Methods
    #region Native
    // Start is called before the first frame update
    void Start()
    {
        _detachableUis = FindObjectsOfType<DetachableUi>();
        if (GetComponent<Canvas>())
            _uiScale = GetComponent<Canvas>().transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(_holdBreakMode))
        {
            SwitchInteractable(false);
            if (_uiElement != null)
                DragShakeLogic();
            else
                IdleShakeLogic();
        }
        else
            SwitchInteractable(true);
    }
    #endregion Native

    #region Custom
    private void DragShakeLogic()
    {
        if (_dragging)
        {

            if (_uiElement.GetComponent<Selectable>() != null)
                _uiElement.GetComponent<Selectable>().interactable = false;

            float distance = Vector2.Distance(_uiElement.InitialPosition, Input.mousePosition);
            if (_uiElement.Attached)
            {
                ShakeElement(_uiElement, distance);
                if (distance >= _maxDistanceBeforeBreaking * _uiScale)
                {
                    _uiElement.Attached = false;
                    Instantiate(_breakParticle, _uiElement.InitialPosition, Quaternion.identity, transform);
                }
            }
            else
            {
                _uiElement.transform.position = Input.mousePosition;
            }
        }
        else if (_uiElement.Attached)
            _uiElement.SetToInitialPosition();
    }

    private void IdleShakeLogic()
    {
        foreach (DetachableUi ui in _detachableUis)
        {
            if(ui.Attached)
                ShakeElement(ui, 0);
        }
    }

    private void SnapBackLogic()
    {
        float distance = Vector2.Distance(_uiElement.InitialPosition, _uiElement.transform.position);
        if (distance < _maxToSnapBack * _uiScale)
        {
            _uiElement.Attached = true;
            _uiElement.SetToInitialPosition();
        }
    }

    private void SwitchInteractable(bool interactible)
    {
        
        if (_breakMode == interactible)
        {
            return;
        }
        else
            _breakMode = interactible;

        foreach (DetachableUi uiElement in _detachableUis)
        {
            if (uiElement.GetComponent<Selectable>() != null)
                uiElement.GetComponent<Selectable>().interactable = interactible;
            if (!_breakMode && uiElement.Attached)
            {
                uiElement.InitialPosition = uiElement.transform.position;
            }
            else if (uiElement.Attached)
                uiElement.SetToInitialPosition();
        }
    }
    private void ShakeElement(DetachableUi target, float distance)
    {
        Vector2 shakePosition = target.InitialPosition + Random.insideUnitCircle * _shakeRate * (_minShake + distance);
        target.transform.position = shakePosition;
    }

    #region EventTrigger
    public void EnterDrag(DetachableUi ui)
    {
        if (!_dragging)
        {
            Debug.Log("enter drag");
            _dragging = true;
            _uiElement = ui;
        }
    }

    public void Drag()
    {
        
    }

    public void Hover(DetachableUi ui)
    {
        if (!_dragging)
        {
            Debug.Log("hover on");
            _uiElement = ui;
        }
    }

    public void Unhover(DetachableUi ui)
    {
        if (!_dragging)
        {
            Debug.Log("hover off");
            _uiElement = null;
        }
    }

    public void ExitDrag()
    {
        if (_dragging)
        {
            Debug.Log("exit drag");
            SnapBackLogic();
            _dragging = false;
            //_uiElement = null;
        }
    }
    #endregion EventTrigger
    #endregion Custom
    #endregion Methods

    private void OnDrawGizmosSelected()
    {
        if (_dragging)
        {
            float shakeRadius = _maxDistanceBeforeBreaking * _uiScale;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_uiElement.InitialPosition, shakeRadius);
            Gizmos.color = Color.red;
            float snapRadius = _maxToSnapBack * _uiScale;
            Gizmos.DrawWireSphere(_uiElement.InitialPosition, snapRadius);
        }
    }
}

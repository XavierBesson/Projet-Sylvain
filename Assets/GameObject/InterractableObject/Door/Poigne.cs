using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poigne : EnigmeObject
{
    [SerializeField] Slider _barATournerUI = null;
    [SerializeField] float _rotationSpeed = 200f;

    [SerializeField] private Sprite _barImageSound = null;

    [SerializeField] private float _minDistance = 5;

    private float _previousAngle;
    private float _totalRotation = 0f;

    [SerializeField] private Door _door;
    private bool canRotate = true;
    private bool _inTransition = false;

    public bool CanRotate { get => canRotate; set => canRotate = value; }
    public bool InTransition { get => _inTransition; set => _inTransition = value; }

    void Start()
    {
        _player = GameManager.Instance.Player;
        _previousAngle = transform.eulerAngles.z;
        _barATournerUI.gameObject.SetActive(false);
        _door.TurnLighting(0);
    }


    public void RotateElement()
    {
        //Check si tu peut intéragir
        if (CanRotate && Vector3.Distance(_player.transform.position, transform.position) <= _minDistance && Input.GetMouseButton(0) && (_door.ObjectOnDoor == EUIObject.VOLUMEBAR || _door.ObjectOnDoor == EUIObject.HEALTHBAR))
        {
            //Vérifie la quantité de rotation
            float currentAngle = transform.eulerAngles.z;
            float delta = Mathf.DeltaAngle(_previousAngle, currentAngle);
            _totalRotation += delta;
            _previousAngle = currentAngle;

            float tours = _totalRotation / 360f;

            if (!_door.Open)
                _door.TurnLighting(tours);


            Ray ray = _player.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 mousePos = hit.point;
                Vector3 direction = mousePos - transform.position;

                
                if (_door.Open)
                {
                    float angle = Mathf.Atan2(direction.z, direction.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 90, angle);
                }
                else
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle + 90);
                }
            }
        }
    }
    

    public void ForceBarToRotate()
    {
        transform.Rotate(new Vector3(0, 0, _rotationSpeed / 6) * Time.deltaTime);
    }


    private void OnMouseOver()
    {
        if (!InTransition)
            RotateElement();
    }



    public override void ActivedObject()
    {
        base.ActivedObject();
        if (_uiObjectToUse != null && Input.GetMouseButtonUp(1))
        {
            switch (_uiObjectToUse.ObjectType)
            {
                case EUIObject.HEALTHBAR:
                    _door.ObjectOnDoor = _uiObjectToUse.ObjectType;
                    _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
                    _barATournerUI.gameObject.SetActive(true);
                    _uiObjectToUse.Despawn();
                    break;

                case EUIObject.VOLUMEBAR:
                    _door.ObjectOnDoor = _uiObjectToUse.ObjectType;
                    _barATournerUI.transform.Find("Background").GetComponent<Image>().sprite = _barImageSound;
                    _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
                    _barATournerUI.gameObject.SetActive(true);
                    _uiObjectToUse.Despawn();
                    break;
            }
            GameManager.Instance.GameLoop -= ActivedObject;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            GameManager.Instance.GameLoop += ActivedObject;
            switch (uiObject.ObjectType)
            {
                case EUIObject.HEALTHBAR:
                    InRangeUIObject(uiObject);
                    break;
                case EUIObject.VOLUMEBAR:
                    InRangeUIObject(uiObject);
                    break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            _uiObjectToUse = null;
            GameManager.Instance.GameLoop -= ActivedObject;
            switch (uiObject.ObjectType)
            {
                case EUIObject.HEALTHBAR:
                    uiObject.HighlightObject(false);
                    break;
                case EUIObject.VOLUMEBAR:
                    uiObject.HighlightObject(false);
                    break;
            }
        }
    }


}
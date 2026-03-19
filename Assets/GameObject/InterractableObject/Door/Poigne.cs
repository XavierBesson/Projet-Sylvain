using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class Poigne : MonoBehaviour
{
    [SerializeField] Slider _barATournerUI = null;
    [SerializeField] float _rotationSpeed = 200f;

    [SerializeField] private Sprite _barImageSound = null;

    [SerializeField] private float _minDistance = 5;

    private float _previousAngle;
    private float _totalRotation = 0f;

    [SerializeField] private Door _door;
    private CharacterController _player;
    private bool canRotate = true;

    public bool CanRotate { get => canRotate; set => canRotate = value; }

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

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (_door.Open)
                    transform.rotation = Quaternion.Euler(0, 90, angle + 90);
                else
                    transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            }
        }
    }
    

    public void ForceBarToRotate()
    {
        transform.Rotate(new Vector3(0, 0, _rotationSpeed / 6) * Time.deltaTime);
    }


    private void OnMouseOver()
    {
        RotateElement();
    }


    private void OnCollisionStay(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            if (Input.GetMouseButtonUp(1))
            {
                switch (uiObject.ObjectType)
                {
                    case EUIObject.HEALTHBAR:
                        _door.ObjectOnDoor = uiObject.ObjectType;
                        _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
                        _barATournerUI.gameObject.SetActive(true);
                        uiObject.Despawn();
                        break;

                    case EUIObject.VOLUMEBAR:
                        _door.ObjectOnDoor = uiObject.ObjectType;
                        _barATournerUI.transform.Find("Background").GetComponent<Image>().sprite = _barImageSound;
                        _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
                        _barATournerUI.gameObject.SetActive(true);
                        uiObject.Despawn();
                        break;
                }
            }  
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            switch (uiObject.ObjectType)
            {
                case EUIObject.HEALTHBAR:
                    uiObject.HighlightObject(true);
                    break;
                case EUIObject.VOLUMEBAR:
                    uiObject.HighlightObject(true);
                    break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
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
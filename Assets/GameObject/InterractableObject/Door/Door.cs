using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;


public class Door : EnigmeObject
{
    [SerializeField] GameObject _entireDoor = null;
    [SerializeField] GameObject _openDoorTransform = null;
    [SerializeField] MeshRenderer _turn1Object = null;
    [SerializeField] MeshRenderer _turn2Object = null;
    [SerializeField] MeshRenderer _turn3Object = null;

    [SerializeField] GameObject _barATourner = null;
    [SerializeField] GameObject _gearObject = null;
    [SerializeField] Slider _barATournerUI = null;
    [SerializeField] float _barprogressSpeed = 0.5f;
    [SerializeField] float _rotationSpeed = 200f;

    [SerializeField] private UnityEngine.Sprite _barImageSound = null;
    [SerializeField] Collider _doorCollider = null;

    private float _previousAngle;
    private float _totalRotation = 0f;

    private EUIObject _objectOnDoor = EUIObject.NONE;

    private bool _inRange = false;
    private bool _open = false;
    


    void Start()
    {
        _previousAngle = _barATourner.transform.eulerAngles.z;
        _gearObject.SetActive(false);
        _barATournerUI.gameObject.SetActive(false);
        TurnLighting(0);
    }


    void Update()
    {
        RotateElement();
        UpdateProgresseBar();
    }

    public void RotateElement()
    {
        //Check si tu peut intéragir
        if (_inRange && Input.GetMouseButton(1) && (_objectOnDoor == EUIObject.VOLUMEBAR || _objectOnDoor == EUIObject.HEALTHBAR))
        {
            //Vérifie la quantité de rotation
            float currentAngle = _barATourner.transform.eulerAngles.z;
            float delta = Mathf.DeltaAngle(_previousAngle, currentAngle);
            _totalRotation += delta;
            _previousAngle = currentAngle;

            float tours = _totalRotation / 360f;

            if (!_open)
                TurnLighting(tours);

            //update la barre uniquement si tu est en porté
            _barATourner.transform.Rotate(new Vector3(0, 0, Input.GetAxis("Mouse Y")) * Time.deltaTime * _rotationSpeed);
        }
    }


    private void UpdateProgresseBar()
    {
        //Prend la rotation de la barre
        float barRotation = _barATourner.transform.eulerAngles.z;

        //Si la rotation est au dessus augment la valeur
        if (barRotation > 130f && barRotation < 230f)
            _barATournerUI.value = _barATournerUI.value - _barprogressSpeed;

        //Si plus petit reduit la valeur
        else if (barRotation < 60f || barRotation > 300f)
            _barATournerUI.value = _barATournerUI.value + _barprogressSpeed;

        UpdateElementThxToDoor();
    }


    public void UpdateElementThxToDoor()
    {
        if (_objectOnDoor == EUIObject.HEALTHBAR)
            _player.Hp = _barATournerUI.value;
        else if (_objectOnDoor == EUIObject.VOLUMEBAR)
            GameManager.Instance.SoundMultiplier = _barATournerUI.value;
    }


    private void TurnLighting(float tours)
    {
        tours = Mathf.Abs(tours);
        if (tours >= 2)
        {
            _turn3Object.material.color = Color.green;
            OpenTheDoor();
            _inRange = false;
            Debug.Log("ouvert");
        }
        else
            _turn3Object.material.color = Color.white;

        if (tours >= 1)
            _turn2Object.material.color = Color.green;
        else
            _turn2Object.material.color = Color.white;

        if (tours >= 0.1f)
            _turn1Object.material.color = Color.green;
        else
            _turn1Object.material.color = Color.white;
    }


    private void ForceBarToRotate()
    {
        _barATourner.transform.Rotate(new Vector3(0, 0, _rotationSpeed / 5) * Time.deltaTime);
    }


    public void SpikeDamage(float damage)
    {
        _barATournerUI.value = _barATournerUI.value - damage;
    }


    public void OpenTheDoor() 
    {
        Debug.Log("ouvert");
        //bouger la porte vers la position et rotation spécifier
        _entireDoor.transform.position = _openDoorTransform.transform.position;
        _entireDoor.transform.rotation = _openDoorTransform.transform.rotation;
        _open = true;
        _doorCollider.enabled = false;
        if (_objectOnDoor == EUIObject.VOLUMEBAR)
        {
            GameManager.Instance.GameLoop += ForceBarToRotate;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        _inRange = true;
    }


    private void OnTriggerExit(Collider other)
    {
        _inRange = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            switch (uiObject.ObjectType)
            {
                case EUIObject.HEALTHBAR:
                    _objectOnDoor = uiObject.ObjectType;
                    _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
                    _barATournerUI.gameObject.SetActive(true);
                    uiObject.Despawn();
                    break;

                case EUIObject.VOLUMEBAR:
                    _objectOnDoor = uiObject.ObjectType;
                    _barATournerUI.transform.Find("Background").GetComponent<Image>().sprite = _barImageSound;
                    _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
                    _barATournerUI.gameObject.SetActive(true);
                    uiObject.Despawn();
                    break;

                case EUIObject.ENGRENAGE:
                    _objectOnDoor = uiObject.ObjectType;
                    _gearObject.SetActive(true);
                    _inRange = false;
                    OpenTheDoor();
                    uiObject.Despawn();
                    break;
            }
        }
    }

}
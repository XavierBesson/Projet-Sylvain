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

    [SerializeField] Collider _doorCollider = null;
    private EUIObject objectOnDoor = EUIObject.NONE;
    private bool open = false;
    [SerializeField] private Poigne _poigné = null;

    private bool _doorOpen = false;


    public bool Open { get => open; set => open = value; }
    public EUIObject ObjectOnDoor { get => objectOnDoor; set => objectOnDoor = value; }

    void Start()
    {
        _player = GameManager.Instance.Player;
        _gearObject.SetActive(false);
        _barATournerUI.gameObject.SetActive(false);
        TurnLighting(0);
    }


    void Update()
    {
        UpdateProgresseBar();
        if (_doorOpen)
        {
            OpenTheDoor();
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
        if (ObjectOnDoor == EUIObject.HEALTHBAR)
            _player.Hp = _barATournerUI.value;
        else if (ObjectOnDoor == EUIObject.VOLUMEBAR)
            GameManager.Instance.SoundMultiplier = _barATournerUI.value;
    }


    public void TurnLighting(float tours)
    {
        tours = Mathf.Abs(tours);
        if (tours >= 2)
        {
            _turn3Object.material.color = Color.green;
            OpenTheDoor();
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


    public void SpikeDamage(float damage)
    {
        _barATournerUI.value = _barATournerUI.value - damage;
    }


    public void OpenTheDoor() 
    {
        Debug.Log("ouvert");
        _entireDoor.transform.position = Vector3.Lerp(_entireDoor.transform.position, _openDoorTransform.transform.position, Time.deltaTime * 10);
        _entireDoor.transform.rotation = Quaternion.Lerp(_entireDoor.transform.rotation, _openDoorTransform.transform.rotation, Time.deltaTime * 15);
        Open = true;
        _doorCollider.enabled = false;
        if (ObjectOnDoor == EUIObject.VOLUMEBAR)
        {
            _poigné.CanRotate = false;
            GameManager.Instance.GameLoop += _poigné.ForceBarToRotate;
        }
    }


    public override void ActivedObject()
    {
        base.ActivedObject();
        if (_uiObjectToUse != null && Input.GetMouseButtonUp(1))
        {
            switch (_uiObjectToUse.ObjectType)
            {
                case EUIObject.ENGRENAGE:
                    ObjectOnDoor = _uiObjectToUse.ObjectType;
                    _gearObject.SetActive(true);
                    //OpenTheDoor();
                    _doorOpen = true;
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
                case EUIObject.ENGRENAGE:
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
                case EUIObject.ENGRENAGE:
                    uiObject.HighlightObject(false);
                    break;
            }
        }
    }

}
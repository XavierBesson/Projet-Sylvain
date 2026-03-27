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

    [SerializeField] private Image _backgroundImage = null;
    [SerializeField] private Image _fillImage = null;
    [SerializeField] private Color _lifeColor;
    [SerializeField] private Color _soundColor;

    private float _previousAngle;
    private float _totalRotation = 0f;

    [SerializeField] private Door _door;
    private bool _canRotate = true;
    private bool _inTransition = false;
    [SerializeField] private AudioClip _placingSound;

    [Header("Animation")]
    [SerializeField] private Animation _twitchAnimation;

    [Header("Cursor Image")]
    [SerializeField] private Texture2D _hoverImage;
    [SerializeField] private Texture2D _unhoverImage;

    public bool CanRotate { get => _canRotate; set => _canRotate = value; }
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
            _twitchAnimation.Stop();
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
        if (CanRotate && (_door.ObjectOnDoor == EUIObject.HEALTHBAR || _door.ObjectOnDoor == EUIObject.VOLUMEBAR))
        {
            Cursor.SetCursor(_hoverImage, Vector2.zero, CursorMode.Auto);
        }
    }

    private void OnMouseExit()
    {
        if (CanRotate && (_door.ObjectOnDoor == EUIObject.HEALTHBAR || _door.ObjectOnDoor == EUIObject.VOLUMEBAR))
        {
            Cursor.SetCursor(_unhoverImage, Vector2.zero, CursorMode.Auto);
        }
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
                    _barATournerUI.gameObject.SetActive(true);
                    _barATournerUI.value = _player.Hp / _player.HpMax;
                    _fillImage.color = _lifeColor;
                    _uiObjectToUse.Despawn();
                    _twitchAnimation.Play();
                    //GameManager.PlaySounds(_door.AudioSource, _placingSound);
                    break;

                case EUIObject.VOLUMEBAR:
                    _door.ObjectOnDoor = _uiObjectToUse.ObjectType;
                    _backgroundImage.sprite = _barImageSound;
                    _barATournerUI.gameObject.SetActive(true);
                    _barATournerUI.value = GameManager.Instance.SoundMultiplier;
                    _fillImage.color = _soundColor;
                    _uiObjectToUse.Despawn();
                    _twitchAnimation.Play();
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
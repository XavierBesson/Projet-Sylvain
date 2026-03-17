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

    CharacterController _player = null;

    [SerializeField] GameObject _barATourner = null;
    [SerializeField] GameObject _gearObject = null;
    [SerializeField] Slider _barATournerUI = null;
    [SerializeField] float _barprogressSpeed = 0.5f;
    [SerializeField] float _rotationSpeed = 200f;

    [SerializeField] private UnityEngine.Sprite _barImageSound = null;
    [SerializeField] private Color _barColor = Color.white;


    [SerializeField] private float _previousAngle;
    [SerializeField] float _totalRotation;

    bool _inRange = false;
    bool _isInteracting = false;
    bool _healthBarUsed = false;
    bool _soundBarUsed = false;
    bool _gearUsed = false;
    bool _open = false;


    float lastAngle;
    float totalRotation = 0f;
    int toursComplets = 0;

    // Start is called before the first frame update
    void Start()
    {
        _previousAngle = _barATourner.transform.eulerAngles.z;
        _totalRotation = 0f;

        _player = GameManager.Instance.Player;

        _gearObject.SetActive(false);

        _barATournerUI.gameObject.SetActive(false);

      _turn1Object.material.color = Color.white;
      _turn2Object.material.color = Color.white;
      _turn3Object.material.color = Color.white;

    }

    // Update is called once per frame
    void Update()
    {
        if (_open)

        {
            _totalRotation = 0f;
            _open = false;
        }
        else
        {
           // Debug.Log(_inRange);
            RotateElement();
        }

        UpdateElementThxToDoor();

    }

    public void RotateElement()
    {
        if (_gearUsed)
        {
            _inRange = false;
            OpenTheDoor();
            Debug.Log("ouvert");
        }

        if (_healthBarUsed == true || _soundBarUsed == true) { 


        //Check si tu peut intéragir
        

            //Vérifie la quantité de rotation
            float currentAngle = _barATourner.transform.eulerAngles.z;
            float delta = Mathf.DeltaAngle(_previousAngle, currentAngle);
            _totalRotation += delta;
            _previousAngle = currentAngle;

            float tours = _totalRotation / 360f;

           
            Debug.Log("tour" + tours);


            //Prend la rotation de la barre
            float barRotation = _barATourner.transform.eulerAngles.z;


            //Si la rotation est au dessus augment la valeur
            if (barRotation > 130f && barRotation < 230f)
            {
                Debug.Log("plusgrand");
                _barATournerUI.value = _barATournerUI.value - _barprogressSpeed; ;
            }

            //Si plus petit reduit la valeur
            else if (barRotation < 60f || barRotation >300f)
            {
                _barATournerUI.value = _barATournerUI.value + _barprogressSpeed;

                Debug.Log("pluspetit");
            }



            if (tours >= 2 || tours <= -2)
            {
                _turn1Object.material.color = Color.green;
                _turn2Object.material.color = Color.green;
                _turn3Object.material.color = Color.green;
            }

            else if (tours >= 1 || tours <= -1)
            {
                _turn1Object.material.color = Color.green;
                _turn2Object.material.color = Color.green;
                _turn3Object.material.color = Color.white;
            }

            else if (tours > 0.1 || tours < -0.1)
            {
                _turn1Object.material.color = Color.green;
                _turn2Object.material.color = Color.white;
                _turn3Object.material.color = Color.white;
            }

            else 
            {
                _turn1Object.material.color = Color.white;
                _turn2Object.material.color = Color.white;
                _turn3Object.material.color = Color.white;
            }










            //Si a bon hauteur ouvre la porte
            if (_totalRotation / 360f >= 2.01f || _totalRotation / 360f <= -2.0f)
            {
                _inRange = false;
                OpenTheDoor();
                Debug.Log("ouvert");
            }

            //update la barre uniquement si tu est en porté
            if (_inRange==true && _isInteracting ==true)
            {

                _barATourner.transform.Rotate(new Vector3(0, 0, Input.GetAxis("Mouse Y")) * Time.deltaTime * _rotationSpeed);




                //  _barATournerUI.value = Input.mousePosition.y/2;
                //  Debug.Log(_barATournerUI.value);

                /*  if (_barATournerUI.value == _barATournerUI.maxValue)
                  { 
                      _canBeInterracted = false;
                      OpenTheDoor();
                  }*/





            }
        }
    }

    public void UpdateElementThxToDoor()
    {
        if (_healthBarUsed == true)
        {

            Debug.Log("HPchange");


            _player.Hp = _barATournerUI.value;
        }
        else if (_soundBarUsed)
        {
            Debug.Log("Audiochange");
        }
    }

    public void SpikeDamage(float damage)
    {
        _barATournerUI.value = _barATournerUI.value - damage;
    }



    public void OpenTheDoor() 
    {
        //bouger la porte vers la position et rotation spécifier
        _entireDoor.transform.position = _openDoorTransform.transform.position;
        _entireDoor.transform.rotation = _openDoorTransform.transform.rotation;
        _open = true;
        _turn1Object.material.color = Color.green;
        _turn2Object.material.color = Color.green;
        _turn3Object.material.color = Color.green;

    }



    private void OnTriggerEnter(Collider other)
    {

        _inRange = true;
        

    }

    private void OnTriggerExit(Collider other)
    {
        _inRange = false;
        _isInteracting = false;
    }

    public void Engrenage()
    {
        _gearUsed = true;
        _gearObject.SetActive(true);
        Debug.Log("Engrenage utilisé");
    }
    public void HpBarUsed()
    {
        _healthBarUsed = true;
        _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
        _barATournerUI.gameObject.SetActive(true);
        UpdateElementThxToDoor();
    }

    public void SoundUsed()
    {
        _soundBarUsed = true;
        _barATournerUI.transform.Find("Background").GetComponent<Image>().sprite = _barImageSound;
        _barATournerUI.transform.Find("Background").GetComponent<Image>().color = Color.white;
        _barATournerUI.gameObject.SetActive(true);
    }


}

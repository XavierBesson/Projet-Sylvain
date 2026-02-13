using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : EnigmeObject
{
    [SerializeField] Canvas _uiDoor = null;
    [SerializeField] GameObject _entireDoor = null;
    [SerializeField] GameObject _openDoorTransform = null;

    [SerializeField] GameObject _barATourner = null;
    [SerializeField] Slider _barATournerUI = null;
    [SerializeField] float _barprogressSpeed = 0.5f;
    [SerializeField] float _rotationSpeed = 200f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (_open)

        {
       
            
        }
        else
        {
           // Debug.Log(_inRange);
            RotateElement();
        }

        float currentAngle = _barATourner.transform.eulerAngles.z;
        float delta = Mathf.DeltaAngle(_previousAngle, currentAngle);
        _totalRotation += delta;
        _previousAngle = currentAngle;

        float tours = _totalRotation / 360f;

        Debug.Log(_totalRotation);
        Debug.Log("tour" + tours);
    }

    public void RevealHint() 
    {

        if (_inRange)
        {
            _uiDoor.gameObject.SetActive(true);
            _isInteracting=true;

        }
        
    }

    public void RotateElement()
    {
        //Check si tu peut intéragir
        if (_isInteracting)
        {
            //Prend la rotation de la barre
            float barRotation = _barATourner.transform.eulerAngles.z;

            //Vérifie la quantité de rotation
           

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
            //Si a bon hauteur ouvre la porte
            if (_totalRotation/360f >=2 || _totalRotation / 360f <=-2)
            {
                _inRange = false;
                OpenTheDoor();
                Debug.Log("ouvert");
            }

            //update la barre uniquement si tu est en porté
            if (_inRange)
            {

                _barATourner.transform.Rotate(new Vector3(0,0, Input.GetAxis("Mouse Y")) * Time.deltaTime * _rotationSpeed);




                //  _barATournerUI.value = Input.mousePosition.y/2;
                //  Debug.Log(_barATournerUI.value);

                /*  if (_barATournerUI.value == _barATournerUI.maxValue)
                  { 
                      _canBeInterracted = false;
                      OpenTheDoor();
                  }*/

                if (_healthBarUsed)
                {

                    //Clilc = obtient
                    //faire un follow 

                }
                else if (_soundBarUsed)
                {
                    //le code de bar qu'on bouge
                }
                else if (_gearUsed)
                {
                    //animation d'ouverture
                }

            }


            //Prend la position souris

            //Prend la rotation objet

            //Quand la souris bouge basé depuis sa position de base l'objet suit la rotation
        }
    }




    public void OpenTheDoor() 
    {
        //bouger la porte vers la position et rotation spécifier
        _entireDoor.transform.position = _openDoorTransform.transform.position;
        _entireDoor.transform.rotation = _openDoorTransform.transform.rotation;
        _open = true; 

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

}

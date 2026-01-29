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

    bool _canBeInterracted = false;
    bool _healthBarUsed = false;
    bool _soundBarUsed = false;
    bool _gearUsed = false;
        bool _open = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_open)

        {
       
            
        }
        else
        {
            Debug.Log(_canBeInterracted);
            RotateElement();
        }




    }

    public void RevealHint() 
    {
    if(_canBeInterracted)
        _uiDoor.gameObject.SetActive(true);
        
    }

    public void RotateElement()
    {

        float barRotation = _barATourner.transform.rotation.z;

        Debug.Log(barRotation); 
        if (barRotation > 0.75f)
        {
            Debug.Log("plusgrand");
            _barATournerUI.value = _barATournerUI.value - _barprogressSpeed; ;
        }
        else if (barRotation < 0.55f)
        {
            _barATournerUI.value = _barATournerUI.value + _barprogressSpeed;
          
            Debug.Log("pluspetit");
        }

        if (barRotation > 0.998f)
        {
            _canBeInterracted = false;
            OpenTheDoor();
            Debug.Log("ouvert");
        }

        if (_canBeInterracted)
        {
            _barATourner.transform.rotation = Quaternion.Euler(0f,0,Input.mousePosition.y);
          

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




    public void OpenTheDoor() 
    {
        //bouger la porte vers la position et rotation spécifier
        _entireDoor.transform.position = _openDoorTransform.transform.position;
        _entireDoor.transform.rotation = _openDoorTransform.transform.rotation;
        _open = true; 

    }



    private void OnTriggerEnter(Collider other)
    {
        _canBeInterracted = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _canBeInterracted = false;
    }

}

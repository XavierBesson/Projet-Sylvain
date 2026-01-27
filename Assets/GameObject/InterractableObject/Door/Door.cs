using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : EnigmeObject
{
    [SerializeField] Canvas _uiDoor = null;
    [SerializeField] GameObject _entireDoor = null;
    [SerializeField] GameObject _openDoorTransform = null; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateElement();
    }

    public void RevealHint() 
    {
    _uiDoor.gameObject.SetActive(true);
        
    }

    public void RotateElement()
    {
        //Prend la position souris

        //Prend la rotation objet

        //Quand la souris bouge basé depuis sa position de base l'objet suit la rotation
    }

    public void OpenTheDoor() 
    {
        //bouger la porte vers la position et rotation spécifier
        _entireDoor.transform.position = _openDoorTransform.transform.position;
        _entireDoor.transform.rotation = _openDoorTransform.transform.rotation;
    }

}

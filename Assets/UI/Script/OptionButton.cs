using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [SerializeField] bool _isOpen = false;
    [SerializeField] GameObject _button; 
   
    Vector3 _originalPosition;
    Vector3 _targetposition;
    [SerializeField] float _positionDistance = 10f; 

    private void Start()
    {
        _originalPosition = transform.position;
        _targetposition = transform.position + (Vector3.right * -_positionDistance);
    }


    public void CheckOpen()
    {
        if (_isOpen == false)
            Open();
        else Close();

    }
            public void Open()
            {

                    _button.transform.position = _targetposition;
                    _isOpen = true;
                }

   public void Close()
    {
        
            gameObject.transform.position = _originalPosition;
            _isOpen = false;
       
    }

}

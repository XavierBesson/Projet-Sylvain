using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [SerializeField] bool _isOpen = false;
    [SerializeField] RectTransform _button;
   
    [SerializeField] RectTransform _optionBox;
    Vector3 _originalPosition;
    Vector3 _targetposition;
    //[SerializeField] float _positionDistance = 10f;

    private void Start()
    {
        _originalPosition = _button.anchoredPosition;
        _targetposition = _button.anchoredPosition + (Vector2.right * -_optionBox.sizeDelta.x);
        Debug.Log(_optionBox.sizeDelta);
    }

    public void CheckOpen()
    {
        if (_isOpen == false)
            Open();
        else Close();
    }
    public void Open()
    {
        _button.anchoredPosition = _targetposition;
        _isOpen = true;
    }

    public void Close()
    {
        _button.anchoredPosition = _originalPosition;
        _isOpen = false;
    }

}

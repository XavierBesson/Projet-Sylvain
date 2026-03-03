using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _sprite;

    public void SetSprite(bool breaked)
    {
        if (breaked)
        {
            _image.sprite = _sprite;
            _image.color = Color.white;
        }
        else
        {
            _image.sprite = null;
            _image.color = Color.clear;
        }
    }
}

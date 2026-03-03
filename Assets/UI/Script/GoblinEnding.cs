using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinEnding : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private float _fadeInRate = 0.01f;
    [SerializeField] private Image _jpegDrawing;
    [SerializeField] private float _fadeInDrawingRate = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEnding()
    {
        if (_background.color.a <= 1)
        {
            Vector4 bColor = _background.color;
            bColor.z += bColor.z + _fadeInRate;
            _background.color = bColor;
        }
        else
        {
            Vector4 jColor = _jpegDrawing.color;
            jColor.z += jColor.z + _fadeInDrawingRate;
            _jpegDrawing.color = jColor;
        }
    }
}

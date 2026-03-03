using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBlade : MonoBehaviour
{
    [SerializeField] private GameObject _sawBlade; 
    private float _speed = 200;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * _speed, Space.Self);
    }
}

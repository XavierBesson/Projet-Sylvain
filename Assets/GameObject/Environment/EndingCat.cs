using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCat : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField]  public float _rotationSpeed = 90f;
    private bool _catStart = false;
    
    [Header("Bobbing")]
    [SerializeField] private float _bobHeight = 0.5f;
    [SerializeField] private float _bobSpeed = 2f;

    

    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        if (_catStart)
        {
            // Rotation sur l'axe Y
            transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);

            // Bobbing haut/bas
            float newY = _startPosition.y + Mathf.Sin(Time.time * _bobSpeed) * _bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public void ActivateCat()
    {
        Invoke("MoveIt", 2);
    }
    public void MoveIt()
    {
        _catStart = true; 
    }
}
